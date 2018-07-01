using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

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
