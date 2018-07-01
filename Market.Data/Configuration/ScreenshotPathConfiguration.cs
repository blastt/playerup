using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{

    public class ScreenshotPathConfiguration : EntityTypeConfiguration<ScreenshotPath>
    {
        public ScreenshotPathConfiguration()
        {
            ToTable("ScreenshotPathes");
            HasRequired(o => o.Offer).WithMany(a => a.ScreenshotPathes).HasForeignKey(a => a.OfferId);

        }
    }
}
