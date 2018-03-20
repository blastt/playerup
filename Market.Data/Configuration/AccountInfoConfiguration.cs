using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    public class AccountInfoConfiguration : EntityTypeConfiguration<AccountInfo>
    {
        public AccountInfoConfiguration()
        {
            ToTable("AccountInfos");
            HasRequired(o => o.Order).WithOptional(a => a.AccountInfo);

        }
    }
}
