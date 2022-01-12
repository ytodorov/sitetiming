using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenshotBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaviconBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Size = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Timings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConnectEnd = table.Column<long>(type: "bigint", nullable: false),
                    ConnectStart = table.Column<long>(type: "bigint", nullable: false),
                    DomComplete = table.Column<long>(type: "bigint", nullable: false),
                    DomContentLoadedEventEnd = table.Column<long>(type: "bigint", nullable: false),
                    DomContentLoadedEventStart = table.Column<long>(type: "bigint", nullable: false),
                    DomInteractive = table.Column<long>(type: "bigint", nullable: false),
                    DomLoading = table.Column<long>(type: "bigint", nullable: false),
                    DomainLookupEnd = table.Column<long>(type: "bigint", nullable: false),
                    DomainLookupStart = table.Column<long>(type: "bigint", nullable: false),
                    FetchStart = table.Column<long>(type: "bigint", nullable: false),
                    LoadEventEnd = table.Column<long>(type: "bigint", nullable: false),
                    LoadEventStart = table.Column<long>(type: "bigint", nullable: false),
                    NavigationStart = table.Column<long>(type: "bigint", nullable: false),
                    RedirectEnd = table.Column<long>(type: "bigint", nullable: false),
                    RedirectStart = table.Column<long>(type: "bigint", nullable: false),
                    RequestStart = table.Column<long>(type: "bigint", nullable: false),
                    ResponseEnd = table.Column<long>(type: "bigint", nullable: false),
                    ResponseStart = table.Column<long>(type: "bigint", nullable: false),
                    SecureConnectionStart = table.Column<long>(type: "bigint", nullable: false),
                    UnloadEventEnd = table.Column<long>(type: "bigint", nullable: false),
                    UnloadEventStart = table.Column<long>(type: "bigint", nullable: false),
                    DOMContentLoadedEventInChrome = table.Column<long>(type: "bigint", nullable: false),
                    LatencyInChrome = table.Column<long>(type: "bigint", nullable: false),
                    LoadEventInChrome = table.Column<long>(type: "bigint", nullable: false),
                    SourceIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccessfull = table.Column<bool>(type: "bit", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    TimetakenToGenerateInMs = table.Column<long>(type: "bigint", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timings_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timings_SiteId",
                table: "Timings",
                column: "SiteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timings");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
