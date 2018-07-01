using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    public class AccountInfoConfiguration : EntityTypeConfiguration<AccountInfo>
    {
        public AccountInfoConfiguration()
        {
            ToTable("AccountInfos");
            HasRequired(o => o.Order).WithMany(a => a.AccountInfos).HasForeignKey(a => a.OrderId);

        }
    }
}
