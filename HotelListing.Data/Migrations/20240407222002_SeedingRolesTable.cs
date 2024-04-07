using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "83cc3825-c940-4bf5-a3dc-f66a4818ed13", null, "Administrator", "ADMINISTRATOR" },
                    { "8ae01be1-ab55-4d54-b04c-0ff3f48cb69a", null, "User", "USER" }
                });
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83cc3825-c940-4bf5-a3dc-f66a4818ed13");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8ae01be1-ab55-4d54-b04c-0ff3f48cb69a");
        }
    }
}
