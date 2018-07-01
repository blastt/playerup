using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    public class BillingConfiguration : EntityTypeConfiguration<Billing>
    {
        public BillingConfiguration()
        {
            ToTable("Billings");

            HasRequired(b => b.User).WithMany(u => u.Billings).HasForeignKey(b => b.UserId);
        }
    }
}