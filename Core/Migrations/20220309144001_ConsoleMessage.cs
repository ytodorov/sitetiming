using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class ConsoleMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressCity",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressCountry",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressHostname",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DestinationIpAddressLatitude",
                table: "Probes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DestinationIpAddressLongitude",
                table: "Probes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressOrg",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressPostal",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressRegion",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DestinationIpAddressTimezone",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DistanceBetweenIpAddresses",
                table: "Probes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressCity",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressCountry",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressHostname",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SourceIpAddressLatitude",
                table: "Probes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SourceIpAddressLongitude",
                table: "Probes",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressOrg",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressPostal",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressRegion",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceIpAddressTimezone",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConsoleMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProbeId = table.Column<int>(type: "int", nullable: false),
                    UniqueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsoleMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsoleMessages_Probes_ProbeId",
                        column: x => x.ProbeId,
                        principalTable: "Probes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsoleMessages_ProbeId",
                table: "ConsoleMessages",
                column: "ProbeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsoleMessages");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressCity",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressCountry",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressHostname",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressLatitude",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressLongitude",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressOrg",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressPostal",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressRegion",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DestinationIpAddressTimezone",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "DistanceBetweenIpAddresses",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressCity",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressCountry",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressHostname",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressLatitude",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressLongitude",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressOrg",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressPostal",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressRegion",
                table: "Probes");

            migrationBuilder.DropColumn(
                name: "SourceIpAddressTimezone",
                table: "Probes");
        }
    }
}
