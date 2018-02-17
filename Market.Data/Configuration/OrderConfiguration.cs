using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Market.Data.Configuration
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");
            Property(o => o.IsFeedbacked).IsRequired();
            Property(o => o.OrderStatus).IsRequired();
            HasRequired(o => o.Offer).WithOptional(o => o.Order).WillCascadeOnDelete(false);


        }
    }
}
