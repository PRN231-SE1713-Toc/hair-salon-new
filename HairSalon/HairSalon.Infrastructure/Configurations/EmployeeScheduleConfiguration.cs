using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Configurations
{
    public class EmployeeScheduleConfiguration : IEntityTypeConfiguration<EmployeeSchedule>
    {
        public void Configure(EntityTypeBuilder<EmployeeSchedule> builder)
        {
            builder.HasOne(x => x.Employee)
                .WithMany(emp => emp.EmployeeSchedules)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
