using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    public class FilterItemConfiguration : EntityTypeConfiguration<FilterItem>
    {
        public FilterItemConfiguration()
        {
            ToTable("FilterItems");

            Property(o => o.Value).IsRequired();
            HasMany(f => f.Offers).WithMany(o => o.FilterItems);


        }
    }
}
