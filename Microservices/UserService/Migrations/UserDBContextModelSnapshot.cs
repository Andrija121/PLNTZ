﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UserService.Data;

#nullable disable

namespace UserService.Migrations
{
    [DbContext(typeof(UserDBContext))]
    partial class UserDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UserService.Identity.User", b =>
                {
                    b.Property<string>("AuthzId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateOnly?>("Birthday")
                        .HasColumnType("date");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Last_seen")
                        .HasColumnType("datetime2");

                    b.HasKey("AuthzId");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            AuthzId = "6567896db88a4affe7295ec2",
                            Birthday = new DateOnly(2001, 1, 26),
                            City = "Eindhoven",
                            Country = "Netherlands",
                            Email = "a.hanga@student.fontys.nl",
                            FirstName = "Andrija",
                            IsActive = true,
                            LastName = "Hanga",
                            Last_seen = new DateTime(2024, 1, 8, 13, 18, 46, 567, DateTimeKind.Utc).AddTicks(5872)
                        },
                        new
                        {
                            AuthzId = "6567896db88a4affe7295ec2123",
                            Birthday = new DateOnly(2001, 1, 26),
                            City = "New York",
                            Country = "The Netherlands",
                            Email = "a.hanga123@student.fontys.nl",
                            FirstName = "Andrija123",
                            IsActive = true,
                            LastName = "Hanga123",
                            Last_seen = new DateTime(2024, 1, 8, 13, 18, 46, 567, DateTimeKind.Utc).AddTicks(5876)
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
