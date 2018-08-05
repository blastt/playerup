using AutoMapper;
using Hangfire;
using Market.Model.Models;
using Market.Service;
using Market.Web.ExtentionMethods;
using Market.Web.Hangfire;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Trader.WEB.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IOrderStatusService _orderStatusService;
        private readonly IBillingService _billingService;
        private readonly ITransactionService _transactionService;
        private readonly IOrderService _orderService;
        private readonly IOfferService _offerService;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IWithdrawService _withdrawService;
        private readonly IIdentityMessageService _identityMessageService;
        public CheckoutController(IUserProfileService userProfileService, IOrderService orderService,
            IOfferService offerService, IAccountInfoService accountInfoService,
            IOrderStatusService orderStatusService, IBillingService billingService,
            ITransactionService transactionService, IIdentityMessageService identityMessageService, IWithdrawService withdrawService)
        {
            _orderStatusService = orderStatusService;
            _userProfileService = userProfileService;
            _orderService = orderService;
            _offerService = offerService;
            _accountInfoService = accountInfoService;
            _billingService = billingService;
            _transactionService = transactionService;
            _identityMessageService = identityMessageService;
            _withdrawService = withdrawService;
        }

        [HttpPost]
        [AllowAnonymous]
        public void MoneyIn()
        {

            var secretKey = "LMcg7uCyA8I3injU";
            var pars = new SortedDictionary<string, string>();
            var keys = Request.Form.AllKeys;
            foreach (var key in keys.Where(key => key.IndexOf("ik_") >= 0 && key != "ik_sign"))
                pars.Add(key, Request.Form[key]);
            var hash = string.Join(":", pars.Select(x => x.Value).ToArray().Concat(new[] { secretKey }));
            var md5 = new MD5CryptoServiceProvider();
            var isSame = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(hash))) == Request.Form["ik_sign"];
            bool isOrderPay = false;
            if (isSame)
            {
                var userId = Request.Form["ik_x_user_id"];
                
                var offerId = Request.Form["ik_x_offer_id"];
                var mainCup = _userProfileService.GetUserProfileByName("palyerup");
                if (offerId != null && userId != null && mainCup != null)
                {
                    var user = _userProfileService.GetUserProfile(u => u.Id == userId, i => i.ApplicationUser);
                    Offer offer = _offerService.GetOffer(int.Parse(offerId), o => o.UserProfile, o => o.UserProfile.ApplicationUser, o => o.Order, o => o.Order.StatusLogs, o => o.Order.CurrentStatus);
                    decimal buyerPay = 0;
                    if (offer.SellerPaysMiddleman)
                    {
                        buyerPay = offer.Price;
                    }
                    else
                    {
                        buyerPay = offer.Price + offer.MiddlemanPrice.Value;

                    }

                    if (buyerPay == Decimal.Parse(Request.Form["ik_am"]))
                    {
                        if (offer != null && offer.UserProfile.Id != User.Identity.GetUserId())
                        {
                            offer.Order = new Order
                            {
                                Buyer = user,
                                Seller = offer.UserProfile,
                                DateCreated = DateTime.Now
                            };
                            Order order = offer.Order;
                            offer.State = OfferState.closed;

                            order.StatusLogs.AddLast(new StatusLog()
                            {
                                OldStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.OrderCreating),
                                NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying),
                                TimeStamp = DateTime.Now
                            });

                            order.StatusLogs.AddLast(new StatusLog()
                            {
                                OldStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying),
                                NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding),
                                TimeStamp = DateTime.Now
                            });
                            order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding);

                            _transactionService.CreateTransaction(new Transaction
                            {
                                Amount = buyerPay,
                                Order = order,
                                Receiver = mainCup,
                                Sender = user,
                                TransactionDate = DateTime.Now
                            });

                            if (offer.JobId != null)
                            {
                                BackgroundJob.Delete(offer.JobId);
                                offer.JobId = null;
                            }
                            _offerService.SaveOffer();
                            isOrderPay = true;
                            MarketHangfire.SetSendEmailChangeStatus(order.Id, offer.UserProfile.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                            MarketHangfire.SetSendEmailChangeStatus(order.Id, user.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                            offer.Order.JobId = MarketHangfire.SetOrderCloseJob(order.Id, TimeSpan.FromDays(1));

                            _orderService.SaveOrder();
                        }

                    }
                }
                // если  пополняем баланс
                
                if (userId != null && !isOrderPay)
                {
                    var user = _userProfileService.GetUserProfile(u => u.Id == userId);
                    var amount = Decimal.Parse(Request.Form["ik_am"]);
                    user.Balance += amount;
                    _billingService.CreateBilling(new Billing
                    {
                        User = user,
                        DateCeated = DateTime.Now,
                        Amount = amount
                    });
                    _userProfileService.SaveUserProfile();
                }                
                else if (!isOrderPay)
                {
                    string currentUserId = User.Identity.GetUserId();
                    var currentUser = _userProfileService.GetUserProfileById(currentUserId);
                    if (currentUser != null)
                    {
                        var amount = Decimal.Parse(Request.Form["ik_am"]);
                        currentUser.Balance += amount;
                        _billingService.CreateBilling(new Billing
                        {
                            User = currentUser,
                            DateCeated = DateTime.Now,
                            Amount = amount
                        });
                        _userProfileService.SaveUserProfile();
                    }
                }
            }
        }
        public ActionResult Success()
        {

            var offerId = Request.Form["ik_x_offer_id"];
            if (offerId != null)
            {
                Offer offer = _offerService.GetOffer(int.Parse(offerId), o => o.UserProfile, o => o.UserProfile.ApplicationUser, o => o.Order);
                if (offer != null)
                {
                    return View("OrderPaidSuccess", (object)offer.Order.Id);
                }
            }
            return View("CashInSuccess");
        }

        public ActionResult Error()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Checkoutme(int? id)
        {
            if (id != null)
            {
                var offer = _offerService.GetOffer(id.Value, o => o.Game, o => o.UserProfile);
                if (offer != null && offer.Order == null && offer.State == OfferState.active && offer.UserProfileId != User.Identity.GetUserId())
                {
                    var userId = User.Identity.GetUserId();
                    var user = _userProfileService.GetUserProfile(u => u.Id == userId);
                    CheckoutViewModel model = new CheckoutViewModel()
                    {
                        OfferHeader = offer.Header,
                        OfferId = offer.Id,
                        Game = offer.Game.Name,
                        SellerPaysMiddleman = offer.SellerPaysMiddleman,
                        MiddlemanPrice = offer.MiddlemanPrice.Value,
                        OrderSum = offer.Price,
                        Quantity = 1,
                        SellerId = offer.UserProfile.Id,
                        BuyerId = userId
                    };
                    if (offer.SellerPaysMiddleman)
                    {
                        model.OrderSum = offer.Price;
                    }
                    else
                    {
                        model.OrderSum = offer.Price + offer.MiddlemanPrice.Value;
                    }
                    model.UserCanPayWithBalance = user.Balance >= model.OrderSum;
                    return View(model);
                }

            }
            return HttpNotFound();
        }
        // GET: Checkout
        // This method must be called after payment
        [HttpPost]
        public async Task<ActionResult> Checkoutme(CheckoutViewModel model)
        {
            //UserProfile buyer = _db.UserProfiles.Get(User.Identity.GetUserId());
            Offer offer = _offerService.GetOffer(model.OfferId, o => o.UserProfile, o => o.UserProfile.ApplicationUser, o => o.Order, o => o.Order.StatusLogs, o => o.Order.CurrentStatus);
            if (offer != null && offer.Order == null && offer.State == OfferState.active && offer.UserProfileId != User.Identity.GetUserId())
            {
                var currentUserId = User.Identity.GetUserId();
                var user = _userProfileService.GetUserProfile(u => u.Id == currentUserId, i => i.ApplicationUser);
                var mainCup = _userProfileService.GetUserProfileByName("palyerup");
                if (user != null && mainCup != null && user.Balance >= model.OrderSum)
                {
                    //var amount = Decimal.Parse(Request.Form["ik_am"]);

                    offer.Order = new Order
                    {
                        BuyerId = currentUserId,
                        SellerId = offer.UserProfileId,
                        DateCreated = DateTime.Now
                    };
                    offer.State = OfferState.closed;


                    if (mainCup != null)
                    {
                        var seller = offer.UserProfile;
                        var buyer = user;

                        if (offer.SellerPaysMiddleman)
                        {
                            offer.Order.Sum = offer.Price;
                            offer.Order.AmmountSellerGet = offer.Price - offer.MiddlemanPrice.Value;
                            if (buyer.Balance >= offer.Order.Sum)
                            {
                                _transactionService.CreateTransaction(new Transaction
                                {
                                    Amount = offer.Order.Sum,
                                    Order = offer.Order,
                                    Receiver = mainCup,
                                    Sender = buyer,
                                    TransactionDate = DateTime.Now
                                });
                                buyer.Balance -= offer.Order.Sum;
                                mainCup.Balance += offer.Order.Sum;
                            }
                            else
                            {
                                return View("NotEnoughMoney");
                            }
                        }
                        else
                        {
                            offer.Order.Sum = offer.Price + offer.MiddlemanPrice.Value;
                            offer.Order.AmmountSellerGet = offer.Price;
                            if (buyer.Balance >= offer.Order.Sum)
                            {
                                _transactionService.CreateTransaction(new Transaction
                                {
                                    Amount = offer.Order.Sum,
                                    Order = offer.Order,
                                    Receiver = mainCup,
                                    Sender = buyer,
                                    TransactionDate = DateTime.Now
                                });
                                buyer.Balance -= offer.Order.Sum;
                                mainCup.Balance += offer.Order.Sum;
                            }
                            else
                            {
                                return View("NotEnoughMoney");
                            }
                        }
                        offer.Order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.OrderCreating),
                            NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying),
                            TimeStamp = DateTime.Now
                        });

                        offer.Order.StatusLogs.AddLast(new StatusLog()
                        {
                            OldStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.BuyerPaying),
                            NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding),
                            TimeStamp = DateTime.Now
                        });
                        offer.Order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding);

                        offer.Order.BuyerChecked = false;
                        offer.Order.SellerChecked = false;

                        if (offer.Order.JobId != null)
                        {
                            BackgroundJob.Delete(offer.Order.JobId);
                            offer.Order.JobId = null;
                        }

                        if (offer.JobId != null)
                        {
                            BackgroundJob.Delete(offer.JobId);
                            offer.JobId = null;
                        }

                        _orderService.SaveOrder();

                        MarketHangfire.SetSendEmailChangeStatus(offer.Order.Id, seller.ApplicationUser.Email, offer.Order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = offer.Order.Id }, protocol: Request.Url.Scheme));

                        MarketHangfire.SetSendEmailChangeStatus(offer.Order.Id, buyer.ApplicationUser.Email, offer.Order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = offer.Order.Id }, protocol: Request.Url.Scheme));
                        offer.Order.JobId = MarketHangfire.SetOrderCloseJob(offer.Order.Id, TimeSpan.FromDays(1));
                        //_orderService.UpdateOrder(order);
                        _orderService.SaveOrder();
                        TempData["orderBuyStatus"] = "Оплата прошла успешно";

                        return RedirectToAction("BuyDetails", "Order", new { id = offer.Order.Id });
                    }

                }
            }
            return HttpNotFound();

        }

        public ActionResult BalanceOperations()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CashIn()
        {
            string userId = User.Identity.GetUserId();
            return View((object)userId);
        }


        

        [HttpGet]
        public ActionResult CashOut()
        {
            return View();
        }


        //public ContentResult Test()
        //{


        //    var request = (HttpWebRequest)WebRequest.Create("https://api.interkassa.com/v1/account");
        //    request.ContentType = "application/json; charset=utf-8";
        //    request.Method = WebRequestMethods.Http.Get;
        //    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("5ac35eb73b1eaf90618b456b:49YehvH0IJr19yCNtmWxBR3TbuwKCCaN")));
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
        //    {
        //        var json = System.Text.RegularExpressions.Regex.Unescape(reader.ReadToEnd());
        //        XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "root");
        //        var res = $"<xml>{doc.InnerXml}</xml>";
        //        return Content(res, "text/xml");
        //    }
        //}

        //public ContentResult TestLocal()
        //{


        //    var request = (HttpWebRequest)WebRequest.Create("https://api.interkassa.com/v1/account");
        //    request.ContentType = "application/json; charset=utf-8";

        //    request.Method = WebRequestMethods.Http.Get;

        //    request.UserAgent = "false";
        //    request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("5ac35eb73b1eaf90618b456b:49YehvH0IJr19yCNtmWxBR3TbuwKCCaN")));
        //    //request.ContentType = "application/x-www-form-urlencoded";

        //    // write the data to the request stream         


        //    // iirc this actually triggers the post
        //    string q = "{'status':'ok','code':0,'data':{'5ac35ee33c1eafbd6d8b4572':{'_id':'5ac35ee33c1eafbd6d8b4572','nm':'\u0411\u0438\u0437\u043d\u0435\u0441 - \u043a\u0430\u0431\u0438\u043d\u0435\u0442','tp':'b','usr':[{'id':'5ac35eb73b1eaf90618b456b','o':'1','rl':'b_o'}]},'5ac35ee33c1eafbd6d8b4571':{'_id':'5ac35ee33c1eafbd6d8b4571','nm':'\u041b\u0438\u0447\u043d\u044b\u0439 \u043a\u0430\u0431\u0438\u043d\u0435\u0442','tp':'c','usr':[{'id':'5ac35eb73b1eaf90618b456b','o':'1','rl':'c_o'}]}}}";


        //    //response.ContentType = "application/json";



        //    var json = System.Text.RegularExpressions.Regex.Unescape(q);
        //    XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "root");
        //    var res = $"<xml>{doc.InnerXml}</xml>";
        //    return Content(res, "text/xml");





        //    //return Content("Error");



        //}

        public JsonResult GetFields(string paywayId)
        {
            //var json = Payments();

            string q = Payments();
            var data = JObject.Parse(q.Replace(@"\", @"\\"));

            var payway = data["data"][paywayId];
            var d = payway["prm"].Children().Count();

            IList<DetailsField> fields = new List<DetailsField>(d);

            foreach (JToken field in payway["prm"].Children())
            {
                string regex = "";
                if (field["re"] != null)
                {
                    regex = field["re"].Value<string>();
                }
                string example = "";
                if (field["ex"] != null)
                {

                    example = field["ex"].Value<string>();
                }
                if (field["tt"] != null && example == "")
                {
                    example = field["tt"].Value<string>();
                }
                string name = "";

                if (field["al"] != null)
                {

                    name = field["al"].Value<string>();
                }
                fields.Add(new DetailsField
                {
                    Regex = regex,
                    Example = example,
                    Name = name
                }
                );
            }

            //var result = JsonConvert.SerializeObject(fields);
            return Json(fields);
        }
        //public async Task<string> GetPurses()
        //{
        //    HttpClient client = new HttpClient();
        //    client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("5ac35eb73b1eaf90618b456b:49YehvH0IJr19yCNtmWxBR3TbuwKCCaN")));
        //    client.DefaultRequestHeaders.Add("Ik-Api-Account-Id", "5ac35ee33c1eafbd6d8b4572");
        //    var response = await client.GetAsync("https://api.interkassa.com/v1/purse");
        //    var responseString = await response.Content.ReadAsStringAsync();

        //    return responseString;
        //}

        [HttpPost]
        public ActionResult Withdraw(CreateWithdrawViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();
                var user = _userProfileService.GetUserProfile(u => u.Id == userId);
                var withdraw = Mapper.Map<CreateWithdrawViewModel, Withdraw>(model);
                withdraw.User = user;
                if (user.Balance >= withdraw.Amount && withdraw.Amount > 50)
                {
                    user.Balance -= withdraw.Amount;
                    
                    _withdrawService.CreateWithdraw(withdraw);
                    _withdrawService.SaveWithdraw();
                    return View("SuccessWithdraw");
                }
                return View("ErrorWithdraw");
            }
            return HttpNotFound();
            //data.Add("details", det);
            //HttpClient client = new HttpClient();
            ////client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            //client.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("5ac35eb73b1eaf90618b456b:49YehvH0IJr19yCNtmWxBR3TbuwKCCaN")));
            //client.DefaultRequestHeaders.Add("Ik-Api-Account-Id", "5ac35ee33c1eafbd6d8b4572");
            //data.Add("calcKey", "ikPayerPrice");
            //data.Add("action", "process");
            //data.Add("paymentNo", Guid.NewGuid().ToString("n"));
            //data.Add("purseId", "407835843341");

            //var q = QueryStringBuilder.BuildQueryString(new
            //{
            //    details = det,
            //    calcKey = "ikPayerPrice",
            //    action = "process",
            //    purseId = "407835843341",
            //    paymentNo = Guid.NewGuid().ToString("n"),
            //    amount = data["amount"],
            //    paywayId = data["paywayId"]
            //});

            //var stringContent = new StringContent(q,
            //             UnicodeEncoding.UTF8,
            //             "application/x-www-form-urlencoded");
            //var resp = await client.PostAsync("https://api.interkassa.com/v1/withdraw", stringContent);
            //var responseString = System.Text.RegularExpressions.Regex.Unescape(await resp.Content.ReadAsStringAsync());
            //var jObject = JObject.Parse(responseString.Replace(@"\", @"\\"));
            //if (jObject["status"].Value<string>() == "ok" && jObject["message"].Value<string>() == "Success")
            //{
            //    decimal amount = Decimal.Parse(jObject["data"]["withdraww"]["psAmount"].Value<string>());
            //}


        }

        [NonAction]
        public string Payments()
        {


            var request = (HttpWebRequest)WebRequest.Create("https://api.interkassa.com/v1/paysystem-output-payway");
            request.Method = WebRequestMethods.Http.Get;
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes("5ac35eb73b1eaf90618b456b:49YehvH0IJr19yCNtmWxBR3TbuwKCCaN")));
            //request.Headers.Add("Ik-Api-Account-Id", "5ac35ee33c1eafbd6d8b4571");

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;



            using (var reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {

                var json = System.Text.RegularExpressions.Regex.Unescape(reader.ReadToEnd());
                //XmlDocument doc = JsonConvert.DeserializeXmlNode(json, "root");
                //var res = $"<xml>{doc.InnerXml}</xml>";
                return json;
            }
            //return Content("Error");
        }
        //[HttpGet]
        //public ActionResult Paid()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult Paid(int? id)
        //{
        //    // Добавить логику оплаты
        //    if (id != null)
        //    {
        //        Order order = _orderService.GetOrder(id.Value, i => i.Seller, i => i.Buyer, i => i.Offer, i => i.CurrentStatus, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser);
        //        if (order != null && order.CurrentStatus.Value == OrderStatuses.BuyerPaying)
        //        {
        //            var mainCup = _userProfileService.GetUserProfileByName("palyerup");
        //            if (mainCup != null)
        //            {
        //                var seller = order.Seller;
        //                var buyer = order.Buyer;

        //                if (order.Offer.SellerPaysMiddleman)
        //                {
        //                    order.Sum = order.Offer.Price;
        //                    order.AmmountSellerGet = order.Offer.Price - order.Offer.MiddlemanPrice.Value;
        //                    if (buyer.Balance >= order.Sum)
        //                    {
        //                        _transactionService.CreateTransaction(new Transaction
        //                        {
        //                            Amount = order.Sum,
        //                            Order = order,
        //                            Receiver = mainCup,
        //                            Sender = buyer,
        //                            TransactionDate = DateTime.Now
        //                        });
        //                        buyer.Balance -= order.Sum;
        //                        mainCup.Balance += order.Sum;
        //                    }
        //                    else
        //                    {
        //                        return View("NotEnoughMoney");
        //                    }
        //                }
        //                else
        //                {
        //                    order.Sum = order.Offer.Price + order.Offer.MiddlemanPrice.Value;
        //                    order.AmmountSellerGet = order.Offer.Price;
        //                    if (buyer.Balance >= order.Offer.Order.Sum)
        //                    {
        //                        _transactionService.CreateTransaction(new Transaction
        //                        {
        //                            Amount = order.Sum,
        //                            Order = order,
        //                            Receiver = mainCup,
        //                            Sender = buyer,
        //                            TransactionDate = DateTime.Now
        //                        });
        //                        buyer.Balance -= order.Sum;
        //                        mainCup.Balance += order.Sum;
        //                    }
        //                    else
        //                    {
        //                        return View("NotEnoughMoney");
        //                    }
        //                }
        //                order.StatusLogs.AddLast(new StatusLog()
        //                {
        //                    OldStatus = order.CurrentStatus,
        //                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding),
        //                    TimeStamp = DateTime.Now
        //                });
        //                order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MiddlemanFinding);

        //                order.BuyerChecked = false;
        //                order.SellerChecked = false;

        //                if (order.JobId != null)
        //                {
        //                    BackgroundJob.Delete(order.JobId);
        //                    order.JobId = null;
        //                }

        //                _orderService.SaveOrder();

        //                MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

        //                MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));
        //                order.JobId = MarketHangfire.SetOrderCloseJob(order.Id, TimeSpan.FromDays(1));
        //                //_orderService.UpdateOrder(order);
        //                _orderService.SaveOrder();
        //                TempData["orderBuyStatus"] = "Оплата прошла успешно";
        //                return RedirectToAction("BuyDetails", "Order", new { id = order.Id });
        //            }

        //        }
        //    }
        //    return View();

        //}

        // этот метод вывызается когда средства были доставлены продавцу
        //[HttpPost]
        //public void SellerWasPaid(int? orderId)
        //{
        //    // Добавить логику оплаты
        //    if (orderId != null)
        //    {
        //        Order order = _orderService.GetOrder(orderId.Value, i => i.CurrentStatus, i => i.StatusLogs);
        //        if (order != null && order.CurrentStatus.Value == OrderStatuses.PayingToSeller)
        //        {
        //            order.StatusLogs.AddLast(new StatusLog()
        //            {
        //                OldStatus = order.CurrentStatus,
        //                NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully),
        //                TimeStamp = DateTime.Now
        //            });
        //            order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.ClosedSuccessfully);
        //            order.BuyerChecked = false;
        //            order.SellerChecked = false;
        //            //_orderService.UpdateOrder(order);
        //            _orderService.SaveOrder();
        //        }
        //    }

        //}



        //private bool CheckSign(HttpContext context)
        //{
        //    var secretKey = "sfewkf342o";
        //    var pars = new SortedDictionary<string, string>();
        //    var keys = context.Request.Form.AllKeys;
        //    foreach (var key in keys.Where(key => key.IndexOf("ik_") >= 0 && key != "ik_sign"))
        //        pars.Add(key, context.Request.Form[key]);
        //    var hash = string.Join(":", pars.Select(x => x.Value).ToArray().Concat(new[] { secretKey }));
        //    var md5 = new MD5CryptoServiceProvider();
        //    return Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(hash))) == context.Request.Form["ik_sign"];
        //}


        //public ActionResult Paid(int? Id, string moderatorId, string buyerId, string sellerId)
        //{
        //    if (Id != null && moderatorId != null && buyerId != null && sellerId != null)
        //    {
        //        Order order = _orderService.GetOrder(Id.Value);
        //        var currentOrderStatus = _orderStatusService.GetCurrentOrderStatus(order);
        //        if (sellerId == User.Identity.GetUserId())
        //        {
        //            if (order.MiddlemanId == moderatorId && order.SellerId == sellerId &&
        //            order.BuyerId == buyerId && currentOrderStatus.Value == "buyerPaying")
        //            {
        //                currentOrderStatus.DateFinished = DateTime.Now;
        //                OrderStatus orderStatus = new OrderStatus
        //                {
        //                    Value = "MiddlemanSearching",
        //                    Name = "Поиск гаранта",
        //                    FinisedName = "Гарант найден",
        //                };
        //                order.BuyerChecked = false;
        //                order.SellerChecked = false;
        //                order.OrderStatuses.AddLast(orderStatus);
        //                _orderService.UpdateOrder(order);
        //                _orderService.SaveOrder();
        //                return View();
        //            }
        //        }

        //    }
        //    return HttpNotFound();

        //}

        // Provide data from seller to moderator
        [HttpGet]
        public ActionResult ProvideData(int? Id, string moderatorId, string buyerId, string sellerId)
        {
            if (Id != null && moderatorId != null && buyerId != null && sellerId != null)
            {
                Order order = _orderService.GetOrder(Id.Value, i => i.Middleman, i => i.Seller, i => i.Buyer, i => i.CurrentStatus, i => i.Offer);
                if (sellerId == User.Identity.GetUserId())
                {
                    if (order.MiddlemanId == moderatorId && order.SellerId == sellerId &&
                    order.BuyerId == buyerId && order.CurrentStatus.Value == OrderStatuses.SellerProviding)
                    {
                        AccountInfoViewModel model = new AccountInfoViewModel
                        {
                            ModeratorId = moderatorId,
                            SteamLogin = order.Offer.AccountLogin,
                            BuyerId = buyerId,
                            SellerId = sellerId

                        };
                        return View(model);
                    }
                }

            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult ProvideData(AccountInfoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var accountInfo = Mapper.Map<AccountInfoViewModel, AccountInfo>(model);

                var moderator = _userProfileService.GetUserProfile(u => u.Id == model.ModeratorId, i => i.ApplicationUser);

                bool moderIsInrole = false;
                if (moderator != null)
                {
                    foreach (var role in moderator.ApplicationUser.Roles)
                    {
                        if (role.RoleId == "2" && role.UserId == moderator.Id)
                        {
                            moderIsInrole = true;
                        }
                    }
                }
                if (moderIsInrole)
                {
                    var order = _orderService.GetOrder(model.SteamLogin, model.ModeratorId, model.SellerId,
                        model.BuyerId, i => i.CurrentStatus, i => i.StatusLogs, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser);
                    if (order != null)
                    {
                        if (order.CurrentStatus != null)
                        {
                            if (order.CurrentStatus.Value == OrderStatuses.SellerProviding)
                            {
                                order.StatusLogs.AddLast(new StatusLog()
                                {
                                    OldStatus = order.CurrentStatus,
                                    NewStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MidddlemanChecking),
                                    TimeStamp = DateTime.Now
                                });
                                order.CurrentStatus = _orderStatusService.GetOrderStatusByValue(OrderStatuses.MidddlemanChecking);


                                order.BuyerChecked = false;
                                order.SellerChecked = false;
                                accountInfo.Order = order;
                                _accountInfoService.CreateAccountInfo(accountInfo);
                                if (order.JobId != null)
                                {
                                    BackgroundJob.Delete(order.JobId);
                                    order.JobId = null;
                                }
                                //order.JobId = MarketHangfire.SetOrderCloseJob(order.Id, TimeSpan.FromMinutes(5));
                                _orderService.SaveOrder();


                                MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                                MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));
                                TempData["orderSellStatus"] = "Ваши данные были отправлены на проверку гаранту";
                                return RedirectToAction("SellDetails", "Order", new { id = order.Id });
                            }
                        }
                    }
                }

            }
            return HttpNotFound();
        }

        public ActionResult ConfirmOrder(int? id)
        {
            if (id != null)
            {
                bool result = _orderService.ConfirmOrder(id.Value, User.Identity.GetUserId());
                var order = _orderService.GetOrder(id.Value, i => i.CurrentStatus, i => i.Seller.ApplicationUser, i => i.Buyer.ApplicationUser);
                if (result && order != null)
                {
                    if (order.JobId != null)
                    {
                        BackgroundJob.Delete(order.JobId);
                        order.JobId = null;
                    }

                    _orderService.SaveOrder();

                    order.JobId = MarketHangfire.SetLeaveFeedbackJob(order.SellerId, order.BuyerId, order.Id, TimeSpan.FromDays(15));


                    MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Seller.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("SellDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                    MarketHangfire.SetSendEmailChangeStatus(order.Id, order.Buyer.ApplicationUser.Email, order.CurrentStatus.DuringName, Url.Action("BuyDetails", "Order", new { id = order.Id }, protocol: Request.Url.Scheme));

                    _orderService.SaveOrder();


                    TempData["orderBuyStatus"] = "Спасибо за подтверждение сделки! Сделка успешно закрыта.";
                    return RedirectToAction("BuyDetails", "Order", new { id });
                }
            }
            return HttpNotFound();
        }


    }
}