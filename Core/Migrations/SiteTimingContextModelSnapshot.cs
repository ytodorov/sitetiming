﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PlaywrightTestLinuxContainer;

#nullable disable

namespace Core.Migrations
{
    [DbContext(typeof(SiteTimingContext))]
    partial class SiteTimingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Core.Entities.ProbeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<long>("ConnectEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("ConnectStart")
                        .HasColumnType("bigint");

                    b.Property<long>("DOMContentLoadedEventInChrome")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<long>("DomComplete")
                        .HasColumnType("bigint");

                    b.Property<long>("DomContentLoadedEventEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("DomContentLoadedEventStart")
                        .HasColumnType("bigint");

                    b.Property<long>("DomInteractive")
                        .HasColumnType("bigint");

                    b.Property<long>("DomLoading")
                        .HasColumnType("bigint");

                    b.Property<long>("DomainLookupEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("DomainLookupStart")
                        .HasColumnType("bigint");

                    b.Property<string>("ExceptionMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExceptionStackTrace")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("FetchStart")
                        .HasColumnType("bigint");

                    b.Property<bool?>("IsSuccessfull")
                        .HasColumnType("bit");

                    b.Property<long>("LatencyInChrome")
                        .HasColumnType("bigint");

                    b.Property<long>("LoadEventEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("LoadEventInChrome")
                        .HasColumnType("bigint");

                    b.Property<long>("LoadEventStart")
                        .HasColumnType("bigint");

                    b.Property<long>("NavigationStart")
                        .HasColumnType("bigint");

                    b.Property<long>("RedirectEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("RedirectStart")
                        .HasColumnType("bigint");

                    b.Property<long>("RequestStart")
                        .HasColumnType("bigint");

                    b.Property<long>("ResponseEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("ResponseStart")
                        .HasColumnType("bigint");

                    b.Property<string>("ScreenshotUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("SecureConnectionStart")
                        .HasColumnType("bigint");

                    b.Property<int>("SiteId")
                        .HasColumnType("int");

                    b.Property<string>("SourceIpAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TimetakenToGenerateInMs")
                        .HasColumnType("bigint");

                    b.Property<Guid?>("UniqueGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("UnloadEventEnd")
                        .HasColumnType("bigint");

                    b.Property<long>("UnloadEventStart")
                        .HasColumnType("bigint");

                    b.Property<string>("VideoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SiteId");

                    b.ToTable("Probes");
                });

            modelBuilder.Entity("Core.Entities.RequestEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<float>("ConnectEnd")
                        .HasColumnType("real");

                    b.Property<float>("ConnectStart")
                        .HasColumnType("real");

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<float>("DomainLookupEnd")
                        .HasColumnType("real");

                    b.Property<float>("DomainLookupStart")
                        .HasColumnType("real");

                    b.Property<string>("Failure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsNavigationRequest")
                        .HasColumnType("bit");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostData")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProbeId")
                        .HasColumnType("int");

                    b.Property<float>("RequestStart")
                        .HasColumnType("real");

                    b.Property<string>("ResourceType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("ResponseEnd")
                        .HasColumnType("real");

                    b.Property<float>("ResponseStart")
                        .HasColumnType("real");

                    b.Property<float>("SecureConnectionStart")
                        .HasColumnType("real");

                    b.Property<float>("StartTime")
                        .HasColumnType("real");

                    b.Property<Guid?>("UniqueGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ProbeId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Core.Entities.SiteEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<string>("FaviconBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScreenshotBase64")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("UniqueGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Core.Entities.ProbeEntity", b =>
                {
                    b.HasOne("Core.Entities.SiteEntity", "Site")
                        .WithMany("Probes")
                        .HasForeignKey("SiteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Site");
                });

            modelBuilder.Entity("Core.Entities.RequestEntity", b =>
                {
                    b.HasOne("Core.Entities.ProbeEntity", "Probe")
                        .WithMany("Requests")
                        .HasForeignKey("ProbeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Probe");
                });

            modelBuilder.Entity("Core.Entities.ProbeEntity", b =>
                {
                    b.Navigation("Requests");
                });

            modelBuilder.Entity("Core.Entities.SiteEntity", b =>
                {
                    b.Navigation("Probes");
                });
#pragma warning restore 612, 618
        }
    }
}
