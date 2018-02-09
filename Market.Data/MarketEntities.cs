using Market.Data.Configuration;
using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace Marketplace.Data
{
    public class MarketEntities : DbContext
    {
        public MarketEntities()
            : base("MarketplaceEntities")
        {
        }

        public virtual void Commit()
        {
            base.SaveChanges();
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
