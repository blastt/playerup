using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    
    public class OrderStatusConfiguration : EntityTypeConfiguration<OrderStatus>
    {
        public OrderStatusConfiguration()
        {
            ToTable("OrderStatus");
            Property(f => f.).IsRequired().HasMaxLength(50);
            Property(f => f.DateLeft).IsRequired();
            Property(f => f.Grade).IsRequired();
            Property(f => f.OfferHeader).IsRequired().HasMaxLength(100);

        }
    }
    
}
