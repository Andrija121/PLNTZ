using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class InitalMigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    AuthzId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    Last_seen = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.AuthzId);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "AuthzId", "Birthday", "City", "Country", "Email", "FirstName", "IsActive", "LastName", "Last_seen" },
                values: new object[,]
                {
                    { "6567896db88a4affe7295ec2", new DateOnly(2001, 1, 26), "Eindhoven", "Netherlands", "a.hanga@student.fontys.nl", "Andrija", true, "Hanga", new DateTime(2024, 1, 8, 13, 18, 46, 567, DateTimeKind.Utc).AddTicks(5872) },
                    { "6567896db88a4affe7295ec2123", new DateOnly(2001, 1, 26), "New York", "The Netherlands", "a.hanga123@student.fontys.nl", "Andrija123", true, "Hanga123", new DateTime(2024, 1, 8, 13, 18, 46, 567, DateTimeKind.Utc).AddTicks(5876) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
