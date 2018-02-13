using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Market.Data.Configuration
{
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
        {
            ToTable("UserProfiles");
            HasMany(u => u.Offers).WithRequired(u => u.UserProfile);
            HasMany(u => u.Orders).WithRequired(u => u.UserProfile);
            HasRequired(o => o.ApplicationUser).WithRequiredDependent(o => o.UserProfile);
        }
    }
}
