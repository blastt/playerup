using AutoMapper;
using Market.Model.Models;
using Market.Service;
using Market.Web.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TinifyAPI;

namespace Market.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        //public ActionResult Panel()
        //{
        //    return View();
        //}
        private readonly IUserProfileService _userProfileService;
        private readonly IOfferService _offerService;
        private readonly IGameService _gameService;
        private readonly IFilterService _filterService;
        private readonly IFilterItemService _filterItemService;
        private readonly IDialogService _dialogService;
        private readonly IWithdrawService _withdrawService;
        private readonly int offerDays = 30;
        public AdminController(IOfferService offerService, IGameService gameService, IFilterService filterService, IDialogService dialogService, IFilterItemService filterItemService, IUserProfileService userProfileService, IWithdrawService withdrawService)
        {
            _offerService = offerService;
            _gameService = gameService;
            _filterService = filterService;
            _filterItemService = filterItemService;
            _userProfileService = userProfileService;
            _dialogService = dialogService;
            _withdrawService = withdrawService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set{}
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult OfferList()
        {
            var model = new OfferListViewModel()
            {
                Offers = Mapper.Map<IEnumerable<Offer>, IEnumerable<OfferViewModel>>(_offerService.GetOffers(i => i.Game, i => i.UserProfile, i => i.Order).ToList())
            };
            return View(model);
        }

        public ActionResult DeleteOffer(int id)
        {
            var offer = _offerService.GetOffer(id);
            if (offer != null)
            {
                _offerService.Delete(offer);
                TempData["message"] = "Предложение удалено";
                _offerService.SaveOffer();
            }
            return RedirectToAction("OfferList");
        }

        public ActionResult GameList()
        {
            IEnumerable<GameViewModel> model = Mapper.Map<IEnumerable<Game>, IEnumerable<GameViewModel>>(_gameService.GetGamesAsNoTracking().ToList());

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateGame()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateGame(CreateGameViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var game = Mapper.Map<CreateGameViewModel, Game>(model);
                if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                {
                    var extName = System.IO.Path.GetExtension(image.FileName);
                    var fileName = $@"{Guid.NewGuid()}{extName}";

                    // сохраняем файл в папку Files в проекте
                    string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\GameIcons", fileName);
                    var urlPath = Url.Content("~/Content/Images/GameIcons/" + fileName);
                    image.SaveAs(fullPath);
                    Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
                    try
                    {
                        using (var s = Tinify.FromFile(fullPath))
                        {
                            var resized = s.Resize(new
                            {
                                method = "fit",
                                width = 40,
                                height = 40
                            });
                            await resized.ToFile(fullPath);
                        }

                    }
                    catch (System.Exception)
                    {
                        // ignored
                    }
                    game.ImagePath = urlPath;
                }
                _gameService.CreateGame(game);

                _gameService.SaveGame();
                return RedirectToAction("GameList");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditGame(int? id)
        {
            if (id != null)
            {
                var game = _gameService.GetGame(id.Value);
                if (game != null)
                {
                    var model = Mapper.Map<Game, EditGameViewModel>(game);
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> EditGame(EditGameViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var game = _gameService.GetGame(model.Id);
                if (game != null)
                {
                    game.Name = model.Name;
                    game.Value = model.Value;
                    game.Rank = model.Rank;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {
                        string extName = System.IO.Path.GetExtension(image.FileName);
                        string fileName = $@"{Guid.NewGuid()}{extName}";

                        // сохраняем файл в папку Files в проекте
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\GameIcons", fileName);
                        string urlPath = Url.Content("~/Content/Images/GameIcons/" + fileName);
                        image.SaveAs(fullPath);
                        Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
                        try
                        {
                            using (var s = Tinify.FromFile(fullPath))
                            {
                                var resized = s.Resize(new
                                {
                                    method = "fit",
                                    width = 40,
                                    height = 40
                                });
                                await resized.ToFile(fullPath);
                            }

                        }
                        catch (System.Exception)
                        {
                            // ignored
                        }
                        game.ImagePath = urlPath;

                    }
                    _gameService.SaveGame();
                    return RedirectToAction("GameList");
                }
            }
            return HttpNotFound();
        }

        public ActionResult DeleteGame(int id)
        {
            Game game = _gameService.GetGame(id);
            if (game != null)
            {
                _gameService.Delete(game);
                TempData["message"] = "Игра удалена";
                _offerService.SaveOffer();
            }
            return RedirectToAction("GameList");
        }

        public ActionResult FilterList()
        {
            IEnumerable<FilterViewModel> model = Mapper.Map<IEnumerable<Model.Models.Filter>, IEnumerable<FilterViewModel>>(_filterService.GetFiltersAsNoTracking(i => i.Game).ToList());

            return View(model);
        }
        [HttpGet]
        public ActionResult CreateFilter()
        {
            CreateFilterViewModel model = new CreateFilterViewModel();
            
            foreach (var game in _gameService.GetGames())
            {
                model.Games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateFilter(CreateFilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var game = _gameService.GetGameByValue(model.Game);
                var filter = Mapper.Map<CreateFilterViewModel, Model.Models.Filter>(model);
                filter.Game = game;
                _filterService.CreateFilter(filter);
                _filterService.SaveFilter();
                return RedirectToAction("FilterList");
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditFilter(int? id)
        {
            if (id != null)
            {
                var filter = _filterService.GetFilter(id.Value);
                if (filter != null)
                {
                    var model = Mapper.Map<Model.Models.Filter, EditFilterViewModel>(filter);
                    foreach (var game in _gameService.GetGames())
                    {
                        model.Games.Add(new SelectListItem { Value = game.Value, Text = game.Name });
                    }
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditFilter(CreateFilterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var filter = _filterService.GetFilter(model.Id);
                var game = _gameService.GetGameByValue(model.Game);
                if (filter != null && game != null)
                {
                    filter.Text = model.Name;
                    filter.Value = model.Value;
                    filter.Game = game;
                    //filter.Ra = model.Rank;
                    _filterService.SaveFilter();
                    return RedirectToAction("FilterItemList");
                }
            }
            return HttpNotFound();
        }

        public ActionResult DeleteFilter(int id)
        {
            var filter = _filterService.GetFilter(id);
            if (filter != null)
            {
                _filterService.Delete(filter);
                TempData["message"] = "Игра удалена";
                _offerService.SaveOffer();
            }
            return RedirectToAction("GameList");
        }

        public ActionResult FilterItemList()
        {
            IEnumerable<FilterItemViewModel> model = Mapper.Map<IEnumerable<FilterItem>, IEnumerable<FilterItemViewModel>>(_filterItemService.GetFilterItemsAsNoTracking(i => i.Filter, i => i.Filter.Game).ToList());
            model = model.OrderBy(m => m.Rank).ThenBy(m => m.FilterName).ThenBy(m => m.GameName);
            return View(model);
        }
        [HttpGet]
        public ActionResult CreateFilterItem()
        {
            CreateFilterItemViewModel model = new CreateFilterItemViewModel();
            foreach (var filter in _filterService.GetFilters(f => true, f => f.Game))
            {
                model.Filters.Add(new SelectListItem()
                {
                    Text = string.Format($"{filter.Text} ({filter.Game.Name})"),
                    Value = filter.Value

                });
            }
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> CreateFilterItem(CreateFilterItemViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var filterItem = Mapper.Map<CreateFilterItemViewModel, FilterItem>(model);
                var filter = _filterService.GetFilterByValue(model.FilterValue);
                if (filter != null)
                {
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {

                        string extName = System.IO.Path.GetExtension(image.FileName);
                        string fileName = $@"{Guid.NewGuid()}{extName}";

                        // сохраняем файл в папку Files в проекте
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\FilterItems", fileName);
                        string urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                        image.SaveAs(fullPath);
                        Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
                        try
                        {

                            using (var s = Tinify.FromFile(fullPath))
                            {
                                var resized = s.Resize(new
                                {
                                    method = "fit",
                                    width = 100,
                                    height = 70
                                });
                                await resized.ToFile(fullPath);
                            }

                        }
                        catch (System.Exception)
                        {
                            // ignored
                        }
                        filterItem.ImagePath = urlPath;
                    }
                    filterItem.Filter = filter;
                    _filterItemService.CreateFilterItem(filterItem);
                    _filterItemService.SaveFilterItem();
                    return RedirectToAction("FilterItemList");
                }
                
            }
            return HttpNotFound();
        }

        [HttpGet]
        public ActionResult EditFilterItem(int? id)
        {
            if (id != null)
            {
                var filterItem = _filterItemService.GetFilterItem(id.Value);
                if (filterItem != null)
                {
                    var model = Mapper.Map<FilterItem, EditFilterItemViewModel>(filterItem);
                    foreach (var filter in _filterService.GetFilters())
                    {
                        model.Filters.Add(new SelectListItem()
                        {
                            Text = string.Format($"{filter.Text} ({filter.Game.Name})"),
                            Value = filter.Value

                        });
                    }
                    return View(model);
                }                                        
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> EditFilterItem(CreateFilterItemViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var filterItem = _filterItemService.GetFilterItem(model.Id);
                var filter = _filterService.GetFilterByValue(model.FilterValue);

                if (filterItem != null && filter != null)
                {
                    filterItem.Name = model.Name;
                    filterItem.Value = model.Value;
                    filterItem.Rank = model.Rank;
                    filterItem.Filter = filter;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {
                        Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
                        var extName = System.IO.Path.GetExtension(image.FileName);
                        var fileName = $@"{Guid.NewGuid()}{extName}";

                        // сохраняем файл в папку Files в проекте
                        string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\FilterItems", fileName);
                        var urlPath = Url.Content("~/Content/Images/FilterItems/" + fileName);
                        image.SaveAs(fullPath);
                        try
                        {
                            using (var s = Tinify.FromFile(fullPath))
                            {
                                var resized = s.Resize(new
                                {
                                    method = "fit",
                                    width = 100,
                                    height = 70
                                });
                                await resized.ToFile(fullPath);
                            }

                        }
                        catch (System.Exception)
                        {
                            // ignored
                        }
                        filterItem.ImagePath = urlPath;
                    }
                    _filterItemService.SaveFilterItem();
                    return RedirectToAction("FilterItemList");
                }
            }
            return HttpNotFound();
        }

        public ActionResult DeleteFilterItem(int id)
        {
            var filter = _filterService.GetFilter(id);
            if (filter != null)
            {
                _filterService.Delete(filter);
                TempData["message"] = "Игра удалена";
                _offerService.SaveOffer();
            }
            return RedirectToAction("GameList");
        }

        public ActionResult Inbox(string sortOrder, string currentFilter, string searchString, string senderName, string id, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewBag.FilterParam = String.IsNullOrEmpty(filter) ? "all" : "";
            ViewData["SenderName"] = senderName;
            ViewData["SearchString"] = searchString;
            ViewData["CurrentFilter"] = currentFilter;
            var currentUser = _userProfileService.GetUserProfileById(id);
            var dialogs = _dialogService.GetUserDialogs(currentUser.Id, i => i.Messages);

            //var messages = _db.Messages.Find(m => m.ReceiverId == User.Identity.GetUserId());

            if (!String.IsNullOrEmpty(searchString))
            {
                //dialogs = dialogs.Where(s => s.MessageBody.Contains(searchString) && s.ReceiverId == User.Identity.GetUserId());
            }
            if (!String.IsNullOrEmpty(senderName))
            {
                //var userProfile = _userProfileService.GetUserProfiles().Where(m => m.ApplicationUser.UserName == senderName).FirstOrDefault();
                //if (userProfile != null)
                //{
                //    messages = messages.Where(s => s.SenderId == userProfile.Id && s.ReceiverId == User.Identity.GetUserId());
                //}
                //else
                //{
                //    messages = Array.Empty<Message>();
                //}

            }

            switch (currentFilter)
            {
                case "read":
                    IList<Dialog> readDialogs = new List<Dialog>();
                    foreach (var dialog in dialogs)
                    {
                        if (dialog.Messages.Any(m => m.ToViewed))
                        {
                            readDialogs.Add(dialog);
                        }

                    }
                    dialogs = readDialogs;
                    break;
                case "unread":
                    IList<Dialog> unreadDialogs = new List<Dialog>();
                    foreach (var dialog in dialogs)
                    {
                        if (dialog.Messages.Any(m => !m.ToViewed))
                        {
                            unreadDialogs.Add(dialog);
                        }

                    }
                    dialogs = unreadDialogs;
                    break;
            }
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        //messages = messages.OrderByDescending(s => s.SenderName);
            //        break;
            //    case "Date":
            //        messages = messages.OrderBy(s => s.CreatedDate);
            //        break;
            //    case "date_desc":
            //        messages = messages.OrderByDescending(s => s.CreatedDate);
            //        break;

            //    default:  // Name ascending 
            //        messages = messages.OrderByDescending(s => s.CreatedDate);
            //        break;
            //}

            DialogListViewModel model = new DialogListViewModel()
            {
                Dialogs = Mapper.Map<IEnumerable<Dialog>, IEnumerable<DialogViewModel>>(dialogs.ToList())


            };

            foreach (var d in model.Dialogs.ToList())
            {
                var otherUserId = _dialogService.GetOtherUserInDialog(d.Id, id);
                var otherUser = _userProfileService.GetUserProfile(u => u.Id == otherUserId);
                if (otherUser != null)
                {
                    d.otherUserId = otherUser.Id;
                    d.otherUserName = otherUser.Name;
                    d.otherUserImage = otherUser.Avatar48Path;
                    d.CountOfNewMessages = d.Messages.Count(m => !m.ToViewed && m.SenderId != currentUser.Id);
                }
            }
            return View(model);
        }

        public ActionResult Details(int? dialogId)
        {
            string currentUserId = User.Identity.GetUserId();
            string dialogWithUserId = null;
            string dialogWithUserImage = null;
            if (dialogId != null)
            {
                Dialog dialog = _dialogService.GetDialog(d => d.Id == dialogId.Value, i => i.Messages, i => i.Companion, i => i.Creator);
                if (dialog != null && (_dialogService.GetUserDialogs(currentUserId).Count() != 0))
                {

                    if (dialog.CompanionId == currentUserId)
                    {
                        dialogWithUserId = dialog.CreatorId;
                        dialogWithUserImage = dialog.Creator.Avatar48Path;
                    }
                    else if (dialog.CreatorId == currentUserId)
                    {
                        dialogWithUserId = dialog.CompanionId;
                        dialogWithUserImage = dialog.Companion.Avatar48Path;
                    }

                    if (dialogWithUserId == null)
                    {
                        return HttpNotFound();
                    }
                    foreach (var message in dialog.Messages.Where(m => m.SenderId != currentUserId))
                    {
                        message.ToViewed = true;
                    }
                    _dialogService.SaveDialog();
                    var model = Mapper.Map<Dialog, DialogViewModel>(dialog);
                    model.otherUserId = dialogWithUserId;
                    model.otherUserImage = dialogWithUserImage;

                    return View(model);
                }
            }
            return HttpNotFound();
        }

        public ActionResult UserList()
        {
            IEnumerable<UserProfileViewModel> model = Mapper.Map<IEnumerable<UserProfile>, IEnumerable<UserProfileViewModel>>(_userProfileService.GetUserProfilesAsNoTracking(i => i.ApplicationUser).ToList());
            model = model.OrderBy(m => m.Name);
            return View(model);
        }

        [HttpGet]
        public ActionResult EditUser(string id)
        {
            if (id != null)
            {
                var user = _userProfileService.GetUserProfile(u => u.Id == id, i => i.ApplicationUser);
                if (user != null)
                {
                    var model = Mapper.Map<UserProfile, EditUserProfileViewModel>(user);
                    return View(model);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(EditUserProfileViewModel model, HttpPostedFileBase image = null)
        {
            if (ModelState.IsValid)
            {
                var user = _userProfileService.GetUserProfile(u => u.Id == model.Id, i => i.ApplicationUser);

                if (user != null)
                {
                    user.Name = model.Name;
                    user.ApplicationUser.UserName = model.Name;
                    user.ApplicationUser.Email = model.Email;
                    user.ApplicationUser.EmailConfirmed = model.EmailConfirmed;
                    user.ApplicationUser.PhoneNumber = model.PhoneNumber;
                    user.ApplicationUser.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
                    user.RegistrationDate = model.RegistrationDate;
                    user.Balance = model.Balance;
                    if (image != null && (image.ContentType == "image/jpeg" || image.ContentType == "image/png"))
                    {
                        string extName = System.IO.Path.GetExtension(image.FileName);
                        string fileName32 = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);
                        string fileName48 = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);
                        string fileName96 = String.Format(@"{0}{1}", System.Guid.NewGuid(), extName);

                        // сохраняем файл в папку Files в проекте
                        string fullPath32 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName32);
                        string urlPath32 = Url.Content("~/Content/Images/Avatars/" + fileName32);
                        string fullPath48 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName48);
                        string urlPath48 = Url.Content("~/Content/Images/Avatars/" + fileName48);
                        string fullPath96 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + "\\Content\\Images\\Avatars", fileName96);
                        string urlPath96 = Url.Content("~/Content/Images/Avatars/" + fileName96);

                        Tinify.Key = ConfigurationManager.AppSettings["TINYPNG_APIKEY"];
                        //Default.png

                        var name32 = user.Avatar32Path.Split('/').LastOrDefault();
                        var name48 = user.Avatar48Path.Split('/').LastOrDefault();
                        var name96 = user.Avatar96Path.Split('/').LastOrDefault();
                        if (name32 != null && name48 != null && name96 != null)
                        {
                            if (name32 != "Default32.png" && name48 != "Default48.png" && name96 != "Default96.png")
                            {
                                System.IO.File.Delete(Server.MapPath(user.Avatar32Path));
                                System.IO.File.Delete(Server.MapPath(user.Avatar48Path));
                                System.IO.File.Delete(Server.MapPath(user.Avatar96Path));
                            }
                        }

                        image.SaveAs(fullPath32);
                        image.SaveAs(fullPath48);
                        image.SaveAs(fullPath96);
                        try
                        {
                            using (var s = Tinify.FromFile(fullPath32))
                            {
                                var resized = s.Resize(new
                                {
                                    method = "fit",
                                    width = 32,
                                    height = 32
                                });
                                await resized.ToFile(fullPath32);
                            }
                            using (var s = Tinify.FromFile(fullPath48))
                            {
                                var resized = s.Resize(new
                                {
                                    method = "fit",
                                    width = 48,
                                    height = 48
                                });
                                await resized.ToFile(fullPath48);
                            }
                            using (var s = Tinify.FromFile(fullPath96))
                            {
                                var resized = s.Resize(new
                                {
                                    method = "fit",
                                    width = 96,
                                    height = 96
                                });
                                await resized.ToFile(fullPath96);
                            }
                        }
                        catch (System.Exception)
                        {
                            // ignored
                        }


                        user.Avatar32Path = urlPath32;
                        user.Avatar48Path = urlPath48;
                        user.Avatar96Path = urlPath96;
                        _userProfileService.SaveUserProfile();
                    }
                    _userProfileService.SaveUserProfile();
                    return RedirectToAction("FilterItemList");
                }
            }
            return HttpNotFound();
        }

        public ActionResult WithdrawList()
        {
            IEnumerable<WithdrawViewModel> model = Mapper.Map<IEnumerable<Withdraw>, IEnumerable<WithdrawViewModel>>(_withdrawService.GetWithdrawsAsNoTracking(i => i.User).ToList());
            model = model.OrderBy(m => m.DateCrated).OrderBy(m => m.UserName);
            return View(model);
        }

        public ActionResult ApplyWithdraw(int? id)
        {
            if (id != null)
            {
                var withdraw = _withdrawService.GetWithdraw(id.Value);
                withdraw.IsPaid = true;
                _withdrawService.SaveWithdraw();
                return RedirectToAction("WithdrawList");
            }
            return HttpNotFound();
        }

        public ActionResult CancelWithdraw(int? id)
        {
            if (id != null)
            {
                var withdraw = _withdrawService.GetWithdraw(id.Value);
                withdraw.IsCanceled = true;
                _withdrawService.SaveWithdraw();
                return RedirectToAction("WithdrawList");
            }
            return HttpNotFound();
        }






        public ActionResult LockoutUserAsync(string id)
        {
            if (id != null)
            {
                var model = new LockoutUserViewModel()
                {
                    UserId = id
                };
                return View(model);
            }
            return HttpNotFound();
        }

        [HttpPost]
        public async Task<ActionResult> LockoutUserAsync(LockoutUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return HttpNotFound();
            }
            var result = await UserManager.SetLockoutEnabledAsync(model.UserId, true);
            TempData["message"] = "Ошибка";
            if (result.Succeeded)
            {
                
                
                var user = _userProfileService.GetUserProfile(u => u.Id == model.UserId, i => i.ApplicationUser);
                user.LockoutReason = model.LockoutReason;
                await UserManager.SetLockoutEndDateAsync(model.UserId, DateTimeOffset.MaxValue);
                var updateStampResult = await UserManager.UpdateSecurityStampAsync(model.UserId);
                _userProfileService.SaveUserProfile();
                if (result.Succeeded && updateStampResult.Succeeded)
                {
                    TempData["message"] = "Пользователь заблокирован";
                }

            }
            return RedirectToAction("UserList");
        }

        public virtual async Task<ActionResult> UnlockUserAccount(string id)
        {
            var result = UserManager.SetLockoutEnabledAsync(id, false);
            var user =_userProfileService.GetUserProfileById(id);
            if (user != null)
            {
                TempData["message"] = "Ошибка";
                if (result.Result.Succeeded)
                {
                    user.LockoutReason = null;
                    _userProfileService.SaveUserProfile();
                    await UserManager.ResetAccessFailedCountAsync(id);
                    TempData["message"] = "Пользователь Разблокирован";
                }
                return RedirectToAction("UserList");
            }

            return HttpNotFound();
        }
    }
}