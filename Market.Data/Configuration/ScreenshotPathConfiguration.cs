using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
