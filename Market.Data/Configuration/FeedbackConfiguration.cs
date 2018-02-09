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
            HasMany(f => f.UserProfiles).WithMany(u => u.Feedbacks);
        }
    }
}
