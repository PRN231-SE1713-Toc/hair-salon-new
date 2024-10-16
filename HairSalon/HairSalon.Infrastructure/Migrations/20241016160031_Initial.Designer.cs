﻿// <auto-generated />
using System;
using HairSalon.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HairSalon.Infrastructure.Migrations
{
    [DbContext(typeof(HairSalonDbContext))]
    [Migration("20241016160031_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HairSalon.Core.Entities.Appointment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateOnly>("AppointmentDate")
                        .HasColumnType("date");

                    b.Property<int>("AppointmentStatus")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<TimeOnly>("EndTime")
                        .HasColumnType("time");

                    b.Property<string>("Note")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<TimeOnly>("StartTime")
                        .HasColumnType("time");

                    b.Property<int>("StylistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StylistId");

                    b.ToTable("Appointments");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.AppointmentService", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("AppServiceId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(10)");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("ServiceId");

                    b.ToTable("AppointmentServices");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)")
                        .HasColumnName("CustomerAddress");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(125)
                        .HasColumnType("nvarchar(125)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)")
                        .HasColumnName("CustomerName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(75)
                        .HasColumnType("nvarchar(75)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St, Anytown, CA 12345",
                            DateOfBirth = new DateTime(1980, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "john.doe@example.com",
                            Name = "John Doe",
                            Password = "securepassword123!",
                            PhoneNumber = "0912345678"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Elm St, Springfield, IL 54321",
                            DateOfBirth = new DateTime(1990, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "jane.smith@email.com",
                            Name = "Jane Smith",
                            Password = "anothersecurepassword!@#",
                            PhoneNumber = "0987654321"
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Maple Ave, Hanoi, Vietnam 10000",
                            DateOfBirth = new DateTime(1975, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "michael.chen@company.com",
                            Name = "Michael Chen",
                            Password = "Myp@ssw0rd1sStr0ng",
                            PhoneNumber = "0901234567"
                        },
                        new
                        {
                            Id = 4,
                            Address = "10 Downing St, London, UK SW1A 2AA",
                            DateOfBirth = new DateTime(2000, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "aisha.khan@gmail.com",
                            Name = "Aisha Khan",
                            Password = "SuperSecurePassw0rd!",
                            PhoneNumber = "0934567890"
                        },
                        new
                        {
                            Id = 5,
                            Address = "Müllerstraße 123, Berlin, Germany 10117",
                            DateOfBirth = new DateTime(1965, 2, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "peter.schmidt@deutschland.de",
                            Name = "Peter Schmidt",
                            Password = "S1ch3rheitsp@ssw0rt!",
                            PhoneNumber = "0956789012"
                        });
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CitizenId")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("EmployeeName");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Le Loi, Ho Chi Minh City",
                            CitizenId = "012345678901",
                            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "nguyen.vana@example.com",
                            Name = "Nguyen Van A",
                            Password = "Nguyen@2024",
                            PhoneNumber = "0901234567",
                            Role = 0
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Nguyen Hue, Ho Chi Minh City",
                            CitizenId = "123456789012",
                            DateOfBirth = new DateTime(1985, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "tran.thib@example.com",
                            Name = "Tran Thi B",
                            Password = "TranThiB@2024",
                            PhoneNumber = "0912345678",
                            Role = 0
                        },
                        new
                        {
                            Id = 3,
                            Address = "789 Tran Hung Dao, Ho Chi Minh City",
                            CitizenId = "234567890123",
                            DateOfBirth = new DateTime(1992, 8, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "le.thic@example.com",
                            Name = "Le Thi C",
                            Password = "LeThiC@2024",
                            PhoneNumber = "0923456789",
                            Role = 1
                        },
                        new
                        {
                            Id = 4,
                            Address = "101 Pham Ngu Lao, Ho Chi Minh City",
                            CitizenId = "345678901234",
                            DateOfBirth = new DateTime(1988, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "pham.vand@example.com",
                            Name = "Pham Van D",
                            Password = "PhamVanD@2024",
                            PhoneNumber = "0934567890",
                            Role = 1
                        },
                        new
                        {
                            Id = 5,
                            Address = "202 Vo Van Kiet, Ho Chi Minh City",
                            CitizenId = "456789012345",
                            DateOfBirth = new DateTime(1995, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "hoang.thie@example.com",
                            Name = "Hoang Thi E",
                            Password = "HoangThiE@2024",
                            PhoneNumber = "0945678901",
                            Role = 1
                        },
                        new
                        {
                            Id = 6,
                            Address = "303 Le Lai, Ho Chi Minh City",
                            CitizenId = "567890123456",
                            DateOfBirth = new DateTime(1992, 4, 15, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "nguyen.minhf@example.com",
                            Name = "Nguyen Minh F",
                            Password = "NguyenMinhF@2024",
                            PhoneNumber = "0956789012",
                            Role = 1
                        },
                        new
                        {
                            Id = 7,
                            Address = "404 Tran Dai Nghia, Ho Chi Minh City",
                            CitizenId = "678901234567",
                            DateOfBirth = new DateTime(1989, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "truong.vang@example.com",
                            Name = "Truong Van G",
                            Password = "TruongVanG@2024",
                            PhoneNumber = "0967890123",
                            Role = 1
                        },
                        new
                        {
                            Id = 8,
                            Address = "",
                            DateOfBirth = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "admin@example.com",
                            Name = "Administrator",
                            Password = "strongpass321!@",
                            Role = 2
                        });
                });

            modelBuilder.Entity("HairSalon.Core.Entities.EmployeeSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("EmpScheduleId");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("WorkingDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("WorkingEndTime")
                        .HasColumnType("time");

                    b.Property<TimeOnly>("WorkingStartTime")
                        .HasColumnType("time");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("EmployeeSchedules");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar")
                        .HasColumnName("ServiceDescription");

                    b.Property<string>("Duration")
                        .HasColumnType("nvarchar(100)")
                        .HasColumnName("EstimatedDuration");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar")
                        .HasColumnName("ServiceName");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10)");

                    b.HasKey("Id");

                    b.ToTable("Services");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.StylistFeedback", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime>("FeedbackDate")
                        .HasColumnType("datetime2");

                    b.Property<short>("Rating")
                        .HasColumnType("smallint");

                    b.Property<int>("StylistId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("StylistId");

                    b.ToTable("StylistFeedbacks");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(10)");

                    b.Property<int>("AppointmentId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("Method")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AppointmentId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Appointment", b =>
                {
                    b.HasOne("HairSalon.Core.Entities.Customer", "Customer")
                        .WithMany("Appointments")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("HairSalon.Core.Entities.Employee", "Stylist")
                        .WithMany("Appointments")
                        .HasForeignKey("StylistId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Stylist");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.AppointmentService", b =>
                {
                    b.HasOne("HairSalon.Core.Entities.Appointment", "Appointment")
                        .WithMany("AppointmentServices")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("HairSalon.Core.Entities.Service", "Service")
                        .WithMany("AppointmentServices")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Service");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.EmployeeSchedule", b =>
                {
                    b.HasOne("HairSalon.Core.Entities.Employee", "Employee")
                        .WithMany("EmployeeSchedules")
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.StylistFeedback", b =>
                {
                    b.HasOne("HairSalon.Core.Entities.Customer", "Customer")
                        .WithMany("StylistFeedback")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("HairSalon.Core.Entities.Employee", "Stylist")
                        .WithMany("StylistFeedback")
                        .HasForeignKey("StylistId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Stylist");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Transaction", b =>
                {
                    b.HasOne("HairSalon.Core.Entities.Appointment", "Appointment")
                        .WithMany("Transactions")
                        .HasForeignKey("AppointmentId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.HasOne("HairSalon.Core.Entities.Customer", "Customer")
                        .WithMany("Transactions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.ClientNoAction)
                        .IsRequired();

                    b.Navigation("Appointment");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Appointment", b =>
                {
                    b.Navigation("AppointmentServices");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Customer", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("StylistFeedback");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Employee", b =>
                {
                    b.Navigation("Appointments");

                    b.Navigation("EmployeeSchedules");

                    b.Navigation("StylistFeedback");
                });

            modelBuilder.Entity("HairSalon.Core.Entities.Service", b =>
                {
                    b.Navigation("AppointmentServices");
                });
#pragma warning restore 612, 618
        }
    }
}
