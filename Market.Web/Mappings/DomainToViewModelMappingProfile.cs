using AutoMapper;
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
            CreateMap<Message, MessageViewModel>();
            CreateMap<Feedback, FeedbackViewModel>();
            CreateMap<UserProfile, UserProfileViewModel>();
        }
    }
}