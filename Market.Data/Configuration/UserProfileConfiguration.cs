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
            HasMany(m => m.FeedbacksAsSeller).WithRequired(m => m.Seller).HasForeignKey(m => m.SellerId).WillCascadeOnDelete(false);
            HasMany(m => m.FeedbacksAsBuyer).WithRequired(m => m.Buyer).HasForeignKey(m => m.BuyerId).WillCascadeOnDelete(false);
            HasMany(m => m.MessagesAsReceiver).WithRequired(m => m.Receiver).HasForeignKey(m => m.ReceiverId).WillCascadeOnDelete(false);
            HasMany(m => m.MessagesAsSender).WithRequired(m => m.Sender).HasForeignKey(m => m.SenderId).WillCascadeOnDelete(false);  
            HasMany(u => u.OrdersAsSeller).WithRequired(u => u.Seller).HasForeignKey(m => m.SellerId).WillCascadeOnDelete(false);
            HasMany(u => u.OrdersAsBuyer).WithOptional(u => u.Buyer).HasForeignKey(m => m.BuyerId).WillCascadeOnDelete(false);
            HasMany(u => u.OrdersAsMiddleman).WithOptional(u => u.Middleman).HasForeignKey(m => m.MiddlemanId).WillCascadeOnDelete(false);
            HasMany(m => m.DialogsAsCreator).WithRequired(m => m.Creator).HasForeignKey(m => m.CreatorId).WillCascadeOnDelete(false);
            HasMany(m => m.DialogsAsСompanion).WithRequired(m => m.Companion).HasForeignKey(m => m.CompanionId).WillCascadeOnDelete(false);
        }
    }
}
