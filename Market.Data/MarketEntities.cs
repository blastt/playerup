using Market.Data.Configuration;
using Market.Model.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Threading.Tasks;

namespace Marketplace.Data
{
    
    public class MarketEntities : IdentityDbContext<ApplicationUser>
    {
        public MarketEntities()
            : base("MarketEntities", throwIfV1Schema: false)
        {
        }

        public  void Commit()
        {
            base.SaveChanges();
        }

        public async virtual Task CommitAsync()
        {
            await base.SaveChangesAsync();
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
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new OfferConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new FeedbackConfiguration());
            modelBuilder.Configurations.Add(new MessageConfiguration());
            modelBuilder.Configurations.Add(new GameConfiguration());
            modelBuilder.Configurations.Add(new FilterConfiguration());
            modelBuilder.Configurations.Add(new FilterItemConfiguration());
            modelBuilder.Configurations.Add(new AccountInfoConfiguration());
            modelBuilder.Configurations.Add(new OrderStatusConfiguration());
            modelBuilder.Configurations.Add(new DialogConfiguration());
            modelBuilder.Configurations.Add(new StatusLogConfiguration());
            modelBuilder.Configurations.Add(new BillingConfiguration());
            modelBuilder.Configurations.Add(new TransactionConfiguration());
            modelBuilder.Configurations.Add(new ScreenshotPathConfiguration());
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<FilterItem> FilterItems { get; set; }
        public DbSet<AccountInfo> AccountInfos { get; set; }
        public DbSet<OrderStatus> OrderStatuses { get; set; }
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<StatusLog> StatusLogs { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ScreenshotPath> ScreenshotPathes { get; set; }
    }
}
