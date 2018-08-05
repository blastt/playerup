using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    public class FeedbackConfiguration : EntityTypeConfiguration<Feedback>
    {
        public FeedbackConfiguration()
        {
            ToTable("Feedbacks");
            Property(f => f.Comment).IsRequired().HasMaxLength(50);
            HasRequired(f => f.Order).WithMany(o => o.Feedbacks).WillCascadeOnDelete(true);
            Property(f => f.Grade).IsRequired();


        }
    }
}
