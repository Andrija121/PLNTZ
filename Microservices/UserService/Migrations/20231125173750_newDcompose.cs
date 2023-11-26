using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Migrations
{
    /// <inheritdoc />
    public partial class newDcompose : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Last_seen",
                value: new DateTime(2023, 11, 25, 17, 37, 49, 723, DateTimeKind.Utc).AddTicks(1529));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                column: "Last_seen",
                value: new DateTime(2023, 11, 24, 19, 39, 57, 377, DateTimeKind.Utc).AddTicks(8115));
        }
    }
}
