using Market.Model.Models;
using System.Data.Entity.ModelConfiguration;

namespace Market.Data.Configuration
{
    public class TransactionConfiguration : EntityTypeConfiguration<Transaction>
    {
        public TransactionConfiguration()
        {
            ToTable("Transactions");

            HasRequired(t => t.Order).WithMany(o => o.Transactions).HasForeignKey(t => t.OrderId).WillCascadeOnDelete(true);

            HasRequired(t => t.Receiver).WithMany(o => o.TransactionsAsReceiver).HasForeignKey(t => t.ReceiverId).WillCascadeOnDelete(false);
            HasRequired(t => t.Sender).WithMany(o => o.TransactionsAsSender).HasForeignKey(t => t.SenderId).WillCascadeOnDelete(false);

        }
    }
}
