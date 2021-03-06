﻿using AutoMapper;
using Market.Model.Models;
using Market.Web.ViewModels;
using System.Linq;

namespace Market.Web.Mappings
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }



        public DomainToViewModelMappingProfile()
        {

            CreateMap<Offer, OfferViewModel>();
            CreateMap<Order, OrderViewModel>();
            CreateMap<Game, GameViewModel>();
            CreateMap<Message, MessageViewModel>();
            CreateMap<Feedback, FeedbackViewModel>();
            CreateMap<UserProfile, UserProfileViewModel>();
            CreateMap<AccountInfo, AccountInfoViewModel>();
            CreateMap<Withdraw, WithdrawViewModel>();
            #region AccountInfo

            CreateMap<AccountInfo, AccountInfoViewModel>()
              .ForMember(o => o.AdditionalInformation, map => map.MapFrom(vm => vm.AdditionalInformation))
              .ForMember(o => o.SteamPassword, map => map.MapFrom(vm => vm.Password))
              .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.Login))
              .ForMember(o => o.SteamEmail, map => map.MapFrom(vm => vm.Email));

            #endregion

            #region Dialog

            CreateMap<Dialog, DialogViewModel>()
               .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))

               .ForMember(o => o.Messages, map => map.MapFrom(vm => vm.Messages))
               .ForMember(o => o.Companion, map => map.MapFrom(vm => vm.Companion))
               .ForMember(o => o.Creator, map => map.MapFrom(vm => vm.Creator))
               .ForMember(o => o.CountOfNewMessages, map => map.MapFrom(vm => vm.Messages.Where(m => !m.ToViewed)));



            #endregion


            #region Offer

            CreateMap<Offer, OfferViewModel>()
               .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
               .ForMember(o => o.SellerPaysMiddleman, map => map.MapFrom(vm => vm.SellerPaysMiddleman))
               .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
               .ForMember(o => o.FilterItems, map => map.MapFrom(vm => vm.FilterItems))
               .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
               .ForMember(o => o.DateDeleted, map => map.MapFrom(vm => vm.DateDeleted))
               .ForMember(o => o.HasOrder, map => map.MapFrom(vm => vm.Order != null))
               .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
               .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
               .ForMember(o => o.UserId, map => map.MapFrom(vm => vm.UserProfileId))
               .ForPath(o => o.Rating, map => map.MapFrom(vm => vm.UserProfile.Rating))
               .ForPath(o => o.UserName, map => map.MapFrom(vm => vm.UserProfile.Name))
               .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
               .ForPath(o => o.User, map => map.MapFrom(vm => vm.UserProfile));

            CreateMap<Offer, CreateOfferViewModel>()
                .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
                .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
                .ForMember(o => o.SellerPaysMiddleman, map => map.MapFrom(vm => vm.SellerPaysMiddleman))
                .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.AccountLogin))
                .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Offer, EditOfferViewModel>()
                .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
                .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
                .ForMember(o => o.SellerPaysMiddleman, map => map.MapFrom(vm => vm.SellerPaysMiddleman))
                .ForMember(o => o.ScreenshotPathes, map => map.MapFrom(vm => vm.ScreenshotPathes))
                .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.AccountLogin))
                .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
                .ForAllOtherMembers(opt => opt.Ignore());

            CreateMap<Offer, DetailsOfferViewModel>()
               .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
               .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
               .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
               .ForMember(o => o.FilterItems, map => map.MapFrom(vm => vm.FilterItems))
               .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
               .ForMember(o => o.UserId, map => map.MapFrom(vm => vm.UserProfileId))
               .ForMember(o => o.ScreenshotPathes, map => map.MapFrom(vm => vm.ScreenshotPathes))
               .ForPath(o => o.Rating, map => map.MapFrom(vm => vm.UserProfile.Rating))
               .ForPath(o => o.UserName, map => map.MapFrom(vm => vm.UserProfile.Name))
               .ForPath(o => o.UserProfile, map => map.MapFrom(vm => vm.UserProfile))
               .ForMember(o => o.Views, map => map.MapFrom(vm => vm.Views))
               .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
               .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
               .ForPath(o => o.Feedbacks, map => map.MapFrom(vm => vm.UserProfile.FeedbacksMy));

            #endregion

            #region Order

            CreateMap<Order, OrderViewModel>()
                .ForMember(o => o.BuyerChecked, map => map.MapFrom(vm => vm.BuyerChecked))

                .ForMember(o => o.SellerChecked, map => map.MapFrom(vm => vm.SellerChecked))
                .ForPath(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForPath(o => o.BuyerName, map => map.MapFrom(vm => vm.Buyer.Name))
                .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
                .ForPath(o => o.OfferHeader, map => map.MapFrom(vm => vm.Offer.Header))
                .ForPath(o => o.CurrentStatus, map => map.MapFrom(vm => vm.CurrentStatus))
                .ForPath(o => o.OfferId, map => map.MapFrom(vm => vm.Offer.Id))
                .ForPath(o => o.OfferPrice, map => map.MapFrom(vm => vm.Offer.Price))
                .ForPath(o => o.SellerName, map => map.MapFrom(vm => vm.Seller.Name));


            CreateMap<Order, DetailsOrderViewModel>()
                .ForMember(o => o.BuyerFeedbacked, map => map.MapFrom(vm => vm.BuyerFeedbacked))
                .ForMember(o => o.SellerFeedbacked, map => map.MapFrom(vm => vm.SellerFeedbacked))
                .ForPath(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.AccountInfos, map => map.MapFrom(vm => vm.AccountInfos))
                .ForMember(o => o.BuyerId, map => map.MapFrom(vm => vm.BuyerId))
                .ForMember(o => o.SellerId, map => map.MapFrom(vm => vm.SellerId))
                .ForPath(o => o.BuyerName, map => map.MapFrom(vm => vm.Buyer.Name))
                .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
                .ForPath(o => o.OfferHeader, map => map.MapFrom(vm => vm.Offer.Header))
                .ForPath(o => o.OfferId, map => map.MapFrom(vm => vm.Offer.Id))
                .ForPath(o => o.OfferPrice, map => map.MapFrom(vm => vm.Offer.Price))
                .ForPath(o => o.ModeratorName, map => map.MapFrom(vm => vm.Middleman.Name))
                .ForPath(o => o.SellerName, map => map.MapFrom(vm => vm.Seller.Name));
            #endregion

            #region FilterItem

            CreateMap<FilterItem, FilterItemViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
               .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
               .ForMember(o => o.Rank, map => map.MapFrom(vm => vm.Rank))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value))
               .ForMember(o => o.ImagePath, map => map.MapFrom(vm => vm.ImagePath))
               .ForPath(o => o.FilterName, map => map.MapFrom(vm => vm.Filter.Text))
               .ForPath(o => o.FilterValue, map => map.MapFrom(vm => vm.Filter.Value))
               .ForPath(o => o.GameName, map => map.MapFrom(vm => vm.Filter.Game.Name));

            CreateMap<FilterItem, EditFilterItemViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
               .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
               .ForMember(o => o.Rank, map => map.MapFrom(vm => vm.Rank))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value))
               .ForMember(o => o.ImagePath, map => map.MapFrom(vm => vm.ImagePath))
               .ForPath(o => o.FilterValue, map => map.MapFrom(vm => vm.Filter.Value))
               .ForPath(o => o.GameName, map => map.MapFrom(vm => vm.Filter.Game.Name));

            #endregion

            #region Filter

            CreateMap<Filter, FilterViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
               .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Text))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value))
               .ForPath(o => o.GameValue, map => map.MapFrom(vm => vm.Game.Value))
               .ForPath(o => o.GameName, map => map.MapFrom(vm => vm.Game.Name));

            CreateMap<Filter, EditFilterViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
               .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Text))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value))
               .ForPath(o => o.Game, map => map.MapFrom(vm => vm.Game.Value));

            #endregion

            //#region Order

            //CreateMap<OrderViewModel, Order>()
            //    .ForMember(o => o.IsFeedbacked, map => map.MapFrom(vm => vm.IsFeedbacked))
            //   .ForMember(o => o.OrderStatus, map => map.MapFrom(vm => vm.OrderStatus))
            //   .ForPath(o => o.UserProfile.Name, map => map.MapFrom(vm => vm.OrderUserName))
            //   .ForPath(o => o.Offer.Id, map => map.MapFrom(vm => vm.OfferId))
            //   .ForPath(o => o.Offer.Header, map => map.MapFrom(vm => vm.OfferName))
            //   .ForPath(o => o.Offer.UserProfile.Name, map => map.MapFrom(vm => vm.OfferUserName))
            //   .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated));

            //#endregion

            #region Message

            CreateMap<Message, MessageViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.ToViewed, map => map.MapFrom(vm => vm.ToViewed))
                .ForPath(o => o.SenderImage, map => map.MapFrom(vm => vm.Sender.Avatar48Path))
                .ForMember(o => o.FromViewed, map => map.MapFrom(vm => vm.FromViewed))
                .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody))
                .ForMember(o => o.ReceiverDeleted, map => map.MapFrom(vm => vm.ReceiverDeleted))
                .ForMember(o => o.SenderDeleted, map => map.MapFrom(vm => vm.SenderDeleted))
                .ForMember(o => o.SenderId, map => map.MapFrom(vm => vm.SenderId))
                .ForMember(o => o.ReceiverId, map => map.MapFrom(vm => vm.ReceiverId))
           
                .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
                .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))
                .ReverseMap()
                .ForMember(o => o.CreatedDate, map => map.MapFrom(vm => vm.CreatedDate));

            CreateMap<Message, NewMessageViewModel > ()
                .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody));

            CreateMap<Message, DetailsMessageViewModel>()
               .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody))              
               .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
               .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))
               .ForMember(o => o.CreatedDate, map => map.MapFrom(vm => vm.CreatedDate));

            #endregion

            #region Feedback

            CreateMap<Feedback, GiveFeedbackViewModel>()
               .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
               .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment));

            CreateMap<Feedback, FeedbackViewModel>()
              .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
              .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment))
              .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.UserFrom.Name))
              .ForPath(o => o.OfferHeader, map => map.MapFrom(vm => vm.Order.Offer.Header));


            //CreateMap<NewFeedbackViewModel, Feedback>()
            //    .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
            //   .ForMember(o => o.OfferHeader, map => map.MapFrom(vm => vm.OfferHeader))
            //   .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment))
            //   .ForMember(o => o.OfferId, map => map.MapFrom(vm => vm.OfferId));

            #endregion

            #region UserProfile

            CreateMap<UserProfile, InfoUserProfileViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.IsOnline, map => map.MapFrom(vm => vm.IsOnline))
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
                .ForMember(o => o.AllFeedbackCount, map => map.MapFrom(vm => vm.AllFeedbackCount))
                .ForMember(o => o.SuccessOrderRate, map => map.MapFrom(vm => vm.SuccessOrderRate))
                .ForMember(o => o.PositiveFeedbacks, map => map.MapFrom(vm => vm.PositiveFeedbackCount))
                .ForMember(o => o.NegativeFeedbacks, map => map.MapFrom(vm => vm.NegativeFeedbackCount))
                .ForMember(o => o.RegistrationDate, map => map.MapFrom(vm => vm.RegistrationDate));

            CreateMap<UserProfile, UserProfileViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForPath(o => o.PhoneNumber, map => map.MapFrom(vm => vm.ApplicationUser.PhoneNumber))
                .ForPath(o => o.Email, map => map.MapFrom(vm => vm.ApplicationUser.Email))
                .ForPath(o => o.RegistrationDate, map => map.MapFrom(vm => vm.RegistrationDate))
                .ForPath(o => o.Balance, map => map.MapFrom(vm => vm.Balance))
                .ForPath(o => o.EmailConfirmed, map => map.MapFrom(vm => vm.ApplicationUser.EmailConfirmed))
                .ForPath(o => o.PhoneNumberConfirmed, map => map.MapFrom(vm => vm.ApplicationUser.PhoneNumberConfirmed))
                .ForPath(o => o.IsBanned, map => map.MapFrom(vm => vm.ApplicationUser.LockoutEndDateUtc != null && vm.ApplicationUser.LockoutEnabled))
                .ForMember(o => o.Rating, map => map.MapFrom(vm => vm.Rating))
                .ForMember(o => o.PositiveFeedbackProcent, map => map.MapFrom(vm => vm.PositiveFeedbackProcent))
                .ForMember(o => o.NegativeFeedbackProcent, map => map.MapFrom(vm => vm.NegativeFeedbackProcent))
                .ForMember(o => o.Positive, map => map.MapFrom(vm => vm.PositiveFeedbackCount))
                .ForMember(o => o.Negative, map => map.MapFrom(vm => vm.NegativeFeedbackCount))
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name));

            CreateMap<UserProfile, EditUserProfileViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForPath(o => o.PhoneNumber, map => map.MapFrom(vm => vm.ApplicationUser.PhoneNumber))
                .ForPath(o => o.Email, map => map.MapFrom(vm => vm.ApplicationUser.Email))
                .ForPath(o => o.RegistrationDate, map => map.MapFrom(vm => vm.RegistrationDate))
                .ForPath(o => o.ImagePath, map => map.MapFrom(vm => vm.Avatar48Path))
                .ForPath(o => o.Balance, map => map.MapFrom(vm => vm.Balance))
                .ForPath(o => o.EmailConfirmed, map => map.MapFrom(vm => vm.ApplicationUser.EmailConfirmed))
                .ForPath(o => o.PhoneNumberConfirmed, map => map.MapFrom(vm => vm.ApplicationUser.PhoneNumberConfirmed))
                .ForPath(o => o.IsBanned, map => map.MapFrom(vm => vm.ApplicationUser.LockoutEndDateUtc != null && vm.ApplicationUser.LockoutEnabled))
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name));


            #endregion

            #region Game
            CreateMap<Game, GameViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.ImagePath, map => map.MapFrom(vm => vm.ImagePath))
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
                .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value));
            #endregion

            #region Transaction
            CreateMap<Transaction, OrderTransactionDetails>()
            .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
            .ForMember(o => o.Amount, map => map.MapFrom(vm => vm.Amount))
            .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
            .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))
            .ForMember(o => o.TransactionDate, map => map.MapFrom(vm => vm.TransactionDate));
            #endregion

            #region Withdraw
            CreateMap<Withdraw, WithdrawViewModel>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.Amount, map => map.MapFrom(vm => vm.Amount))
                .ForMember(o => o.Details, map => map.MapFrom(vm => vm.Details))
                .ForMember(o => o.DateCrated, map => map.MapFrom(vm => vm.DateCrated))
                .ForPath(o => o.UserName, map => map.MapFrom(vm => vm.User.Name))
                .ForMember(o => o.PaywayName, map => map.MapFrom(vm => vm.PaywayName));
            #endregion
        }
    }
}