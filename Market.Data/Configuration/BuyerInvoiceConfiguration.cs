using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    public class BuyerInvoiceConfiguration : EntityTypeConfiguration<BuyerInvoice>
    {
        public BuyerInvoiceConfiguration()
        {
            ToTable("BuyerInvoices");

            HasRequired(b => b.User).WithMany(u => u.BuyerInvoices).HasForeignKey(b => b.UserId);
        }
    }
}
