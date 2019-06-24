using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TicketData.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeatClaims",
                columns: table => new
                {
                    SeatClaimId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(nullable: true),
                    TimeStampHeld = table.Column<DateTime>(nullable: true),
                    TimeStampReserved = table.Column<DateTime>(nullable: true),
                    HoldId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatClaims", x => x.SeatClaimId);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    SeatId = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SeatRow = table.Column<string>(nullable: true),
                    SeatNum = table.Column<string>(nullable: true),
                    Rank = table.Column<float>(nullable: false),
                    SeatClaimId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.SeatId);
                    table.ForeignKey(
                        name: "FK_Seats_SeatClaims_SeatClaimId",
                        column: x => x.SeatClaimId,
                        principalTable: "SeatClaims",
                        principalColumn: "SeatClaimId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SeatClaimId",
                table: "Seats",
                column: "SeatClaimId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "SeatClaims");
        }
    }
}
