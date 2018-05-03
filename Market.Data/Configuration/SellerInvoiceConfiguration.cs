using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    public class SellerInvoiceConfiguration : EntityTypeConfiguration<SellerInvoice>
    {
        public SellerInvoiceConfiguration()
        {
            ToTable("SellerInvoices");

            HasRequired(b => b.User).WithMany(u => u.SellerInvoices).HasForeignKey(b => b.UserId);

        }
    }
}
