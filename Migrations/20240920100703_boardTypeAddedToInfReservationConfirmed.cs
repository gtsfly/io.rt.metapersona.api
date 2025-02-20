using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace otel_advisor_webApp.Migrations
{
    /// <inheritdoc />
    public partial class boardTypeAddedToInfReservationConfirmed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "board_type",
                table: "inf_reservation_confirmed",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "board_type",
                table: "inf_reservation_");
        }
    }
}
