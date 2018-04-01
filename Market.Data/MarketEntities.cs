using Market.Data.Configuration;
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
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Configuration.ProxyCreationEnabled = false;
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new OfferConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new FilterConfiguration());
            modelBuilder.Configurations.Add(new FilterItemConfiguration());
            modelBuilder.Configurations.Add(new AccountInfoConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Offer> Offers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Filter> Filters { get; set; }
        public virtual DbSet<FilterItem> FilterItems { get; set; }
        public virtual DbSet<AccountInfo> AccountInfos { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
    }
}
