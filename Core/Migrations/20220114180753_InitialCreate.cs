using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ScreenshotBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaviconBase64 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sites", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Probes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceIpAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsSuccessfull = table.Column<bool>(type: "bit", nullable: true),
                    ExceptionMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExceptionStackTrace = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SiteId = table.Column<int>(type: "int", nullable: false),
                    TimetakenToGenerateInMs = table.Column<long>(type: "bigint", nullable: true),
                    ScreenshotUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UniqueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    LoadEventInChrome = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Probes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Probes_Sites_SiteId",
                        column: x => x.SiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Failure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsNavigationRequest = table.Column<bool>(type: "bit", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResourceType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProbeId = table.Column<int>(type: "int", nullable: false),
                    UniqueGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartTime = table.Column<float>(type: "real", nullable: false),
                    DomainLookupStart = table.Column<float>(type: "real", nullable: false),
                    DomainLookupEnd = table.Column<float>(type: "real", nullable: false),
                    ConnectStart = table.Column<float>(type: "real", nullable: false),
                    SecureConnectionStart = table.Column<float>(type: "real", nullable: false),
                    ConnectEnd = table.Column<float>(type: "real", nullable: false),
                    RequestStart = table.Column<float>(type: "real", nullable: false),
                    ResponseStart = table.Column<float>(type: "real", nullable: false),
                    ResponseEnd = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requests_Probes_ProbeId",
                        column: x => x.ProbeId,
                        principalTable: "Probes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Probes_SiteId",
                table: "Probes",
                column: "SiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_ProbeId",
                table: "Requests",
                column: "ProbeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Probes");

            migrationBuilder.DropTable(
                name: "Sites");
        }
    }
}
