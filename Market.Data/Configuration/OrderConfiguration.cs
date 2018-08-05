using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    public class OrderConfiguration : EntityTypeConfiguration<Order>
    {
        public OrderConfiguration()
        {
            ToTable("Orders");
            Property(o => o.WithmiddlemanSum).HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Computed);
            HasRequired(o => o.Offer).WithOptional(o => o.Order).WillCascadeOnDelete(true);
            HasRequired(o => o.CurrentStatus).WithMany(s => s.Orders).HasForeignKey(o => o.CurrentStatusId).WillCascadeOnDelete(true); ;

        }
    }
}
