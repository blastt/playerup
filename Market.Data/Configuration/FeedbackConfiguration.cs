using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Market.Data.Configuration
{
    public class FeedbackConfiguration : EntityTypeConfiguration<Feedback>
    {
        public FeedbackConfiguration()
        {
            ToTable("Feedbacks");
            Property(f => f.Comment).IsRequired().HasMaxLength(50);
            Property(f => f.DateLeft).IsRequired();
            Property(f => f.Grade).IsRequired();
            Property(f => f.OfferHeader).IsRequired().HasMaxLength(100);
            HasRequired(m => m.Sender).WithMany(m => m.Feedbacks).HasForeignKey(m => m.SenderId);
            HasRequired(m => m.Receiver).WithMany(m => m.Feedbacks).HasForeignKey(m => m.ReceiverId);
        }
    }
}
