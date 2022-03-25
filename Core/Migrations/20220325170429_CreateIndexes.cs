using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class CreateIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Sites",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Sites",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sites",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SourceIpAddress",
                table: "Probes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DestinationIpAddress",
                table: "Probes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sites_Name",
                table: "Sites",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_Title",
                table: "Sites",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Sites_Url",
                table: "Sites",
                column: "Url");

            migrationBuilder.CreateIndex(
                name: "IX_Probes_DestinationIpAddress",
                table: "Probes",
                column: "DestinationIpAddress");

            migrationBuilder.CreateIndex(
                name: "IX_Probes_SourceIpAddress",
                table: "Probes",
                column: "SourceIpAddress");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sites_Name",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_Title",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Sites_Url",
                table: "Sites");

            migrationBuilder.DropIndex(
                name: "IX_Probes_DestinationIpAddress",
                table: "Probes");

            migrationBuilder.DropIndex(
                name: "IX_Probes_SourceIpAddress",
                table: "Probes");

            migrationBuilder.AlterColumn<string>(
                name: "Url",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sites",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SourceIpAddress",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DestinationIpAddress",
                table: "Probes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
