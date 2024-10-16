using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Data
{
    public static class CustomerSeeder
    {
        public static void SeedDataForCustomer(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "John Doe",
                    Email = "john.doe@example.com",
                    Address = "123 Main St, Anytown, CA 12345",
                    Password = "securepassword123!",
                    PhoneNumber = "0912345678",
                    DateOfBirth = new DateTime(1980, 1, 1)
                },
                new Customer
                {
                    Id = 2,
                    Name = "Jane Smith",
                    Email = "jane.smith@email.com",
                    Address = "456 Elm St, Springfield, IL 54321",
                    Password = "anothersecurepassword!@#",
                    PhoneNumber = "0987654321",
                    DateOfBirth = new DateTime(1990, 5, 15)
                },
                new Customer
                {
                    Id = 3,
                    Name = "Michael Chen",
                    Email = "michael.chen@company.com",
                    Address = "789 Maple Ave, Hanoi, Vietnam 10000",
                    Password = "Myp@ssw0rd1sStr0ng",
                    PhoneNumber = "0901234567",
                    DateOfBirth = new DateTime(1975, 12, 24)
                },
                new Customer
                {
                    Id = 4,
                    Name = "Aisha Khan",
                    Email = "aisha.khan@gmail.com",
                    Address = "10 Downing St, London, UK SW1A 2AA",
                    Password = "SuperSecurePassw0rd!",
                    PhoneNumber = "0934567890",
                    DateOfBirth = new DateTime(2000, 8, 8)
                },
                new Customer
                {
                    Id = 5,
                    Name = "Peter Schmidt",
                    Email = "peter.schmidt@deutschland.de",
                    Address = "Müllerstraße 123, Berlin, Germany 10117",
                    Password = "S1ch3rheitsp@ssw0rt!",
                    PhoneNumber = "0956789012",
                    DateOfBirth = new DateTime(1965, 2, 11)
                }
            );
        }
    }
}
