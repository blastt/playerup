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
            HasRequired(f => f.Order).WithMany(o => o.Feedbacks).WillCascadeOnDelete(false);
            Property(f => f.Grade).IsRequired();


        }
    }
}
