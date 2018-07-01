using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    class GameConfiguration : EntityTypeConfiguration<Game>
    {
        public GameConfiguration()
        {
            ToTable("Games");

            Property(o => o.Name).IsRequired().HasMaxLength(100);
            Property(o => o.Value).IsRequired();
            HasMany(g => g.Offers).WithRequired(o => o.Game).HasForeignKey(o => o.GameId).WillCascadeOnDelete(false);
            
        }
    }
}
