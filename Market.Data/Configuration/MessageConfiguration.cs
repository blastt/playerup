using Market.Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Text;

namespace Market.Data.Configuration
{
    public class MessageConfiguration : EntityTypeConfiguration<Message>
    {
        public MessageConfiguration()
        {
            ToTable("Messages");
            Property(m => m.MessageBody).IsRequired().HasMaxLength(200);
            HasRequired(m => m.Sender).WithMany(m => m.Messages).HasForeignKey(m => m.SenderId);
            HasRequired(m => m.Receiver).WithMany(m => m.Messages).HasForeignKey(m => m.ReceiverId);
        }
    }
}
