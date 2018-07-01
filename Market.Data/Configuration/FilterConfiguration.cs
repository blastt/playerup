using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    class FilterConfiguration : EntityTypeConfiguration<Filter>
    {
        public FilterConfiguration()
        {
            ToTable("Filters");

            Property(o => o.Value).IsRequired();
            HasRequired(o => o.Game).WithMany(o => o.Filters).WillCascadeOnDelete(false);
            HasMany(f => f.Offers).WithMany(o => o.Filters);
            HasMany(f => f.FilterItems).WithRequired(f => f.Filter);

        }
    }
}
