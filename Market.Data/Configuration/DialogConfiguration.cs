using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

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
