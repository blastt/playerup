using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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