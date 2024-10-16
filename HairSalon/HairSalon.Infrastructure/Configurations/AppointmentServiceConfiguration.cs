using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Configurations
{
    public class AppointmentServiceConfiguration : IEntityTypeConfiguration<AppointmentService>
    {
        public void Configure(EntityTypeBuilder<AppointmentService> builder)
        {
            builder.HasOne(x => x.Service)
                .WithMany(y => y.AppointmentServices)
                .HasForeignKey(x => x.ServiceId)
                .OnDelete(DeleteBehavior.ClientNoAction);

            builder.HasOne(x => x.Appointment)
                .WithMany(y => y.AppointmentServices)
                .HasForeignKey(x => x.AppointmentId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
