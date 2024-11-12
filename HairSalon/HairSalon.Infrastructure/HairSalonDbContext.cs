using HairSalon.Core.Entities;
using HairSalon.Infrastructure.Configurations;
using HairSalon.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HairSalon.Infrastructure
{
    public class HairSalonDbContext : DbContext
    {
        public HairSalonDbContext() {}

        public HairSalonDbContext(DbContextOptions<HairSalonDbContext> options) : base(options) { }

        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<AppointmentService> AppointmentServices { get; set; }
        
        public DbSet<Customer> Customers { get; set; }
        
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<EmployeeSchedule> EmployeeSchedules { get; set; }
        
        public DbSet<Service> Services { get; set; }
        
        public DbSet<StylistFeedback> StylistFeedbacks { get; set; }
        
        public DbSet<Transaction> Transactions { get; set; }

        private string GetConnectionString()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                //.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
                .Build();

            return configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //if (optionsBuilder)
            optionsBuilder.UseSqlServer("Data Source=(local);Initial Catalog=HairSalon;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False", sqlOptions => sqlOptions.CommandTimeout(120));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Entities configuration
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppointmentServiceConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StylistFeedbackConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeScheduleConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionConfiguration).Assembly);

            // Data seeding
            modelBuilder.SeedDataForCustomer();
            modelBuilder.SeedDataForEmployees();
            modelBuilder.SeedDataForService();
            modelBuilder.SeedDataForStylistFeedback();
        }
    }
}
