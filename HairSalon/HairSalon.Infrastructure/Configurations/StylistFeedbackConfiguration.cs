using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Configurations
{
    public class StylistFeedbackConfiguration : IEntityTypeConfiguration<StylistFeedback>
    {
        public void Configure(EntityTypeBuilder<StylistFeedback> builder)
        {
            builder.HasOne(x => x.Customer)
                .WithMany(y => y.StylistFeedback)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Stylist)
                .WithMany(y => y.StylistFeedback)
                .HasForeignKey(x => x.StylistId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
