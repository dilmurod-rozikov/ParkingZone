using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingZoneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ParkingSlotAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryType",
                table: "ParkingSlot",
                newName: "Tariff");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tariff",
                table: "ParkingSlot",
                newName: "CategoryType");
        }
    }
}
