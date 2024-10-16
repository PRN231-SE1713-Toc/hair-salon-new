using HairSalon.Core.Entities;
using HairSalon.Core.Enums;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Data
{
    public static class EmployeeSeeder
    {
        public static void SeedDataForEmployees(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Nguyen Van A",
                    Email = "nguyen.vana@example.com",
                    Password = "Nguyen@2024",
                    PhoneNumber = "0901234567",
                    CitizenId = "012345678901",
                    Address = "123 Le Loi, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    Role = EmployeeRole.STAFF
                },
                new Employee
                {
                    Id = 2,
                    Name = "Tran Thi B",
                    Email = "tran.thib@example.com",
                    Password = "TranThiB@2024",
                    PhoneNumber = "0912345678",
                    CitizenId = "123456789012",
                    Address = "456 Nguyen Hue, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1985, 5, 15),
                    Role = EmployeeRole.STAFF
                },
                new Employee
                {
                    Id = 3,
                    Name = "Le Thi C",
                    Email = "le.thic@example.com",
                    Password = "LeThiC@2024",
                    PhoneNumber = "0923456789",
                    CitizenId = "234567890123",
                    Address = "789 Tran Hung Dao, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1992, 8, 25),
                    Role = EmployeeRole.STYLIST
                },
                new Employee
                {
                    Id = 4,
                    Name = "Pham Van D",
                    Email = "pham.vand@example.com",
                    Password = "PhamVanD@2024",
                    PhoneNumber = "0934567890",
                    CitizenId = "345678901234",
                    Address = "101 Pham Ngu Lao, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1988, 3, 10),
                    Role = EmployeeRole.STYLIST
                },
                new Employee
                {
                    Id = 5,
                    Name = "Hoang Thi E",
                    Email = "hoang.thie@example.com",
                    Password = "HoangThiE@2024",
                    PhoneNumber = "0945678901",
                    CitizenId = "456789012345",
                    Address = "202 Vo Van Kiet, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1995, 12, 5),
                    Role = EmployeeRole.STYLIST
                },
                new Employee
                {
                    Id = 6,
                    Name = "Nguyen Minh F",
                    Email = "nguyen.minhf@example.com",
                    Password = "NguyenMinhF@2024",
                    PhoneNumber = "0956789012",
                    CitizenId = "567890123456",
                    Address = "303 Le Lai, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1992, 4, 15),
                    Role = EmployeeRole.STYLIST
                },
                new Employee
                {
                    Id = 7,
                    Name = "Truong Van G",
                    Email = "truong.vang@example.com",
                    Password = "TruongVanG@2024",
                    PhoneNumber = "0967890123",
                    CitizenId = "678901234567",
                    Address = "404 Tran Dai Nghia, Ho Chi Minh City",
                    DateOfBirth = new DateTime(1989, 11, 20),
                    Role = EmployeeRole.STYLIST
                },
                new Employee
                {
                    Id = 8,
                    Name = "Administrator",
                    Email = "admin@example.com",
                    Password = "strongpass321!@",
                    Address = string.Empty,
                    DateOfBirth = DateTime.MinValue,
                    Role = EmployeeRole.ADMIN
                }
            );
        }
    }
}
