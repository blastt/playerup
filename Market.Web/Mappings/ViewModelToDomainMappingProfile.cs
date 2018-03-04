using AutoMapper;
using Market.Model.Models;
using Market.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Market.Web.Mappings
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        public ViewModelToDomainMappingProfile()
        {
            #region Offer

            //CreateMap<OfferViewModel, Offer>()
            //   .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
            //   .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
            //   .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
            //   .ForPath(o => o.UserProfile.Name, map => map.MapFrom(vm => vm.UserName))
               
            //   .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price));

            CreateMap<CreateOfferViewModel, Offer>()
                .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
                .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
                .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.SteamLogin))
                .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
                .ForAllOtherMembers(opt => opt.Ignore());

            //CreateMap<EditOfferViewModel, Offer>()
            //    .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
            //    .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
            //    .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
            //    .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
            //    .ForMember(o => o.SteamLogin, map => map.MapFrom(vm => vm.SteamLogin))
            //    .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price));

            //CreateMap<DetailsOfferViewModel, Offer>()
            //   .ForMember(o => o.Header, map => map.MapFrom(vm => vm.Header))
            //   .ForMember(o => o.Discription, map => map.MapFrom(vm => vm.Discription))
            //   .ForMember(o => o.Filters, map => map.MapFrom(vm => vm.Filters))
            //   .ForMember(o => o.Game, map => map.MapFrom(vm => vm.Game))
            //   .ForPath(o => o.UserProfile.Name, map => map.MapFrom(vm => vm.UserName))
            //   .ForMember(o => o.Views, map => map.MapFrom(vm => vm.Views))
            //   .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated))
            //   .ForMember(o => o.Price, map => map.MapFrom(vm => vm.Price))
            //   .ForPath(o => o.UserProfile.Feedbacks, map => map.MapFrom(vm => vm.Feedbacks));

            #endregion

            #region Order

            CreateMap<OrderViewModel, Order>()
                .ForMember(o => o.IsFeedbacked, map => map.MapFrom(vm => vm.IsFeedbacked))
               .ForMember(o => o.OrderStatus, map => map.MapFrom(vm => vm.OrderStatus))
               .ForPath(o => o.UserProfile.Name, map => map.MapFrom(vm => vm.OrderUserName))
               .ForPath(o => o.Offer.Id, map => map.MapFrom(vm => vm.OfferId))
               .ForPath(o => o.Offer.Header, map => map.MapFrom(vm => vm.OfferName))
               .ForPath(o => o.Offer.UserProfile.Name, map => map.MapFrom(vm => vm.OfferUserName))
               .ForMember(o => o.DateCreated, map => map.MapFrom(vm => vm.DateCreated));

            #endregion

            #region Message

            CreateMap<MessageViewModel, Message>()
               .ForMember(o => o.IsViewed, map => map.MapFrom(vm => vm.IsViewed))
               .ForMember(o => o.MessageBody, map => map.MapFrom(vm => vm.MessageBody))
               .ForMember(o => o.ReceiverDeleted, map => map.MapFrom(vm => vm.ReceiverDeleted))
               .ForMember(o => o.SenderDeleted, map => map.MapFrom(vm => vm.SenderDeleted))
               .ReverseMap()
               .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))              
               .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
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
               .ForPath(o => o.ReceiverName, map => map.MapFrom(vm => vm.Receiver.Name))
               .ForPath(o => o.SenderName, map => map.MapFrom(vm => vm.Sender.Name))
               .ReverseMap()
               .ForMember(o => o.CreatedDate, map => map.MapFrom(vm => vm.CreatedDate));

            #endregion

            #region Feedback

            CreateMap<FeedbackViewModel, Feedback>()
               .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
               .ForMember(o => o.OfferHeader, map => map.MapFrom(vm => vm.OfferHeader))
               .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment))
               .ForMember(o => o.DateLeft, map => map.MapFrom(vm => vm.DateLeft));


            CreateMap<NewFeedbackViewModel, Feedback>()
                .ForMember(o => o.Grade, map => map.MapFrom(vm => vm.Grade))
               .ForMember(o => o.OfferHeader, map => map.MapFrom(vm => vm.OfferHeader))
               .ForMember(o => o.Comment, map => map.MapFrom(vm => vm.Comment))
               .ForMember(o => o.OfferId, map => map.MapFrom(vm => vm.OfferId));

            #endregion

            #region Game            

            CreateMap<CreateGameViewModel, Game>()
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value));

            #endregion

            #region Game            

            CreateMap<CreateFilterViewModel, Filter>()
                .ForMember(o => o.Text, map => map.MapFrom(vm => vm.Name))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value));

            #endregion

            #region Game            

            CreateMap<CreateFilterItemViewModel, FilterItem>()
                .ForMember(o => o.Name, map => map.MapFrom(vm => vm.Name))
               .ForMember(o => o.Value, map => map.MapFrom(vm => vm.Value));

            #endregion

            CreateMap<EditOfferViewModel, Order>();

        }
    }
}