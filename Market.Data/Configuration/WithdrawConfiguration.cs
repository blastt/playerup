using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Data.Configuration
{
    public class WithdrawConfiguration : EntityTypeConfiguration<Withdraw>
    {
        public WithdrawConfiguration()
        {
            ToTable("Withdraws");

            HasRequired(b => b.User).WithMany(u => u.Withdraws).HasForeignKey(b => b.UserId);
        }
    }
}
