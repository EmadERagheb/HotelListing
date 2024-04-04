using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HotelListing.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedHotalAndCountryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "Name", "ShortName", "UpdateBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Jamaica", "JM", "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bahamas", "BS", "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cayman Island", "CI", "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "Address", "CountryId", "CreatedBy", "CreatedDate", "Name", "Rating", "UpdateBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Negril", 1, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sandals Resort and Spa", 4.5, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "George Town", 3, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Comfort Suites", 4.2999999999999998, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Nassua", 2, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grand Palldium", 4.0, "Emad Ragheb", new DateTime(2024, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Hotels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
