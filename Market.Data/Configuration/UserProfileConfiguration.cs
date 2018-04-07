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
            
            HasRequired(o => o.ApplicationUser).WithRequiredDependent(o => o.UserProfile).WillCascadeOnDelete(false);
            HasMany(m => m.Feedbacks).WithRequired(m => m.Sender).HasForeignKey(m => m.SenderId);
            HasMany(m => m.Feedbacks).WithRequired(m => m.Receiver).HasForeignKey(m => m.ReceiverId);
            HasMany(m => m.Messages).WithRequired(m => m.Sender).HasForeignKey(m => m.SenderId);
            HasMany(m => m.Messages).WithRequired(m => m.Receiver).HasForeignKey(m => m.ReceiverId);
            HasMany(u => u.Orders).WithRequired(u => u.Buyer).HasForeignKey(m => m.BuyerId);
            HasMany(u => u.Orders).WithRequired(u => u.Seller).HasForeignKey(m => m.SellerId);
            HasMany(u => u.Orders).WithOptional(u => u.Moderator).HasForeignKey(m => m.ModeratorId);
            HasMany(m => m.Dialogs).WithMany(m => m.Users);


        }
    }
}
