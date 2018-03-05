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

        }
    }
}
