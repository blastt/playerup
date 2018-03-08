using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Market.Data.Configuration
{
    public class OfferConfiguration : EntityTypeConfiguration<Offer>
    {
        public OfferConfiguration()
        {
            ToTable("Offers");

            Property(o => o.Header).IsRequired().HasMaxLength(100);
            Property(o => o.Discription).IsRequired().HasMaxLength(1000);
            Property(o => o.Price).IsRequired().HasPrecision(8,2);
            Property(o => o.SteamLogin).IsRequired().HasMaxLength(50);
            
        }
    }
}
