using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingZoneApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationModelNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ParkingSlot_ParkingSlotId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_ParkingSlotId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "ParkingSlotId",
                table: "Reservation");

            migrationBuilder.AddColumn<Guid>(
                name: "SlotId",
                table: "Reservation",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reservation",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_SlotId",
                table: "Reservation",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_AspNetUsers_UserId",
                table: "Reservation",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_ParkingSlot_SlotId",
                table: "Reservation",
                column: "SlotId",
                principalTable: "ParkingSlot",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_AspNetUsers_UserId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_ParkingSlot_SlotId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_SlotId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_UserId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "SlotId",
                table: "Reservation");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reservation");

            migrationBuilder.AddColumn<Guid>(
                name: "ParkingSlotId",
                table: "Reservation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_ParkingSlotId",
                table: "Reservation",
                column: "ParkingSlotId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_ParkingSlot_ParkingSlotId",
                table: "Reservation",
                column: "ParkingSlotId",
                principalTable: "ParkingSlot",
                principalColumn: "Id");
        }
    }
}
