using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    public class DialogConfiguration : EntityTypeConfiguration<Dialog>
    {
        public DialogConfiguration()
        {
            ToTable("Dialogs");
            HasMany(m => m.Messages).WithRequired(m => m.Dialog).HasForeignKey(m => m.DialogId);

        }
    }
}
