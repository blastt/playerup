﻿using AutoMapper;
using Market.Model.Models;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

            #region Offer

            CreateMap<Offer, OfferViewModel>()
               .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
               .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
               .ForMember(o => o.FilterItems, map => map.MapFrom(vm => vm.FilterItems))
               .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
               .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
               .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
               .ForPath(o => o.User, map => map.MapFrom(vm => vm.UserProfile));

            CreateMap<Offer, CreateOfferViewModel> ()
                .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
                .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
                .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
                .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.SteamLogin))
                .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price));

            //CreateMap<Offer, EditOfferViewModel>()
            //    .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
            //    .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
            //    .ForMember(o => o.Filter, map => map.MapFrom(vm => vm.Filter))
            //    .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
            //    .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.SteamLogin))
            //    .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price));

            CreateMap<Offer, DetailsOfferViewModel>()
               .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
               .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
               .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
               .ForMember(o => o.FilterItems, map => map.MapFrom(vm => vm.FilterItems))
               .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
               .ForPath(o => o.UserProfile, map => map.MapFrom(vm => vm.UserProfile))
               .ForMember(o => o.Views, map => map.MapFrom(vm => vm.Views))
               .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
               .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
               .ForPath(o => o.Feedbacks, map => map.MapFrom(vm => vm.UserProfile.Feedbacks));

            #endregion

            #region FilterItem

            CreateMap<FilterItem, FilterItemViewModel>()
               .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value))
               .ForMember(o => o.Image, map => map.MapFrom(vm => vm.Image));

            #endregion

            #region Filter

            CreateMap<Filter, FilterViewModel>()
               .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Text))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value))
               .ForPath(o => o.GameValue, map => map.MapFrom(vm => vm.Game.Value));

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

            CreateMap<MessageViewModel, Message>()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.IsViewed, map => map.MapFrom(vm => vm.IsViewed))
                .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody))
                .ForMember(o => o.ReceiverDeleted, map => map.MapFrom(vm => vm.ReceiverDeleted))
                .ForMember(o => o.SenderDeleted, map => map.MapFrom(vm => vm.SenderDeleted))
                .ForMember(o => o.SenderId, map => map.MapFrom(vm => vm.SenderId))
                .ForMember(o => o.ReceiverId, map => map.MapFrom(vm => vm.ReceiverId))
                .ReverseMap()
                .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
                .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))
                .ReverseMap()
                .ForMember(o => o.CreatedDate, map => map.MapFrom(vm => vm.CreatedDate))
                .ForMember(o => o.Subject, map => map.MapFrom(vm => vm.Subject));

            CreateMap<NewMessageViewModel, Message>()
                .ForMember(o => o.Subject, map => map.MapFrom(vm => vm.Subject))
                .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody));

            CreateMap<DetailsMessageViewModel, Message>()
               .ForMember(o => o.Subject, map => map.MapFrom(vm => vm.Subject))
               .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody))
               .ReverseMap()
               .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
               .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))
               .ReverseMap()
               .ForMember(o => o.CreatedDate, map => map.MapFrom(vm => vm.CreatedDate));

            #endregion

            //#region Feedback

            //CreateMap<FeedbackViewModel, Feedback>()
            //   .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
            //   .ForMember(o => o.OfferHeader, map => map.MapFrom(vm => vm.OfferHeader))
            //   .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment))
            //   .ForMember(o => o.DateLeft, map => map.MapFrom(vm => vm.DateLeft));


            //CreateMap<NewFeedbackViewModel, Feedback>()
            //    .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
            //   .ForMember(o => o.OfferHeader, map => map.MapFrom(vm => vm.OfferHeader))
            //   .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment))
            //   .ForMember(o => o.OfferId, map => map.MapFrom(vm => vm.OfferId));

            //#endregion

            #region UserProfile

            CreateMap<UserProfile, InfoUserProfileViewModel > ()
                .ForMember(o => o.Id, map => map.MapFrom(vm => vm.Id))
                .ForMember(o => o.Avatar, map => map.MapFrom(vm => vm.Avatar))
                .ForMember(o => o.PositiveFeedbacks, map => map.MapFrom(vm => vm.Positive))
                .ForMember(o => o.NegativeFeedbacks, map => map.MapFrom(vm => vm.Negative))
                .ForMember(o => o.IsOnline, map => map.MapFrom(vm => vm.IsOnline))
                .ForMember(o => o.Rating, map => map.MapFrom(vm => vm.Rating))
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
                .ForMember(o => o.RegistrationDate, map => map.MapFrom(vm => vm.RegistrationDate));

            #endregion
        }
    }
}