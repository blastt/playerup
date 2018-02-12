﻿using Market.Data.Configuration;
using Market.Model.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Data
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }
    public class MarketEntities : IdentityDbContext<ApplicationUser>
    {
        public MarketEntities()
            : base("MarketEntities", throwIfV1Schema: false)
        {
        }

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        public static MarketEntities Create()
        {
            return new MarketEntities();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ApplicationUser>().HasOptional(m => m.ClientProfile)
        //        .WithRequired(m => m.ApplicationUser).WillCascadeOnDelete(true);
        //    modelBuilder.Entity<ClientProfile>().HasKey(m=>m.Id).HasOptional(m => m.Offers).WithMany()
        //        .WillCascadeOnDelete(true);
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new OfferConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
        }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}
