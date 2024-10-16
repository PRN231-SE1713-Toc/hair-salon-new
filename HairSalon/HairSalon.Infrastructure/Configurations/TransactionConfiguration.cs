using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasOne(x => x.Customer)
                .WithMany(y => y.Transactions)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Appointment)
                .WithMany(y => y.Transactions)
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
