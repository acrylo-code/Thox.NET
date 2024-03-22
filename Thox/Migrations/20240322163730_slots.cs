using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thox.Migrations
{
    /// <inheritdoc />
    public partial class slots : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationSlots_Rooms_RoomID",
                table: "ReservationSlots");

            migrationBuilder.DropIndex(
                name: "IX_ReservationSlots_RoomID",
                table: "ReservationSlots");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ReservationSlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "StartTime",
                table: "ReservationSlots",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.CreateIndex(
                name: "IX_ReservationSlots_RoomID",
                table: "ReservationSlots",
                column: "RoomID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationSlots_Rooms_RoomID",
                table: "ReservationSlots",
                column: "RoomID",
                principalTable: "Rooms",
                principalColumn: "RoomID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
