using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace PlaywrightTestLinuxContainer
{
    public class SiteTimingContext : DbContext
    {
        public DbSet<SiteEntity>? Sites { get; set; }
        public DbSet<ProbeEntity>? Probes { get; set; }
        public DbSet<RequestEntity>? Requests { get; set; }

        public DbSet<ConsoleMessageEntity>? ConsoleMessages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=tcp:hximbq2jrp.database.windows.net,1433;Initial Catalog=websitetimings;Persist Security Info=False;User ID=aYordan;Password=123Pass!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
#if (DEBUG)
            connectionString = "Data Source=.\\SQLDEVELOPER2019;Initial Catalog=sitetiming;Integrated Security=True;MultipleActiveResultSets=true;";
#endif
            //optionsBuilder.LogTo(Console.WriteLine, (eventId, logLevel) => eventId == RelationalEventId.CommandExecuted);

            optionsBuilder.UseSqlServer(connectionString, o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SiteEntity>()
               .HasIndex(b => b.Title);

            modelBuilder.Entity<SiteEntity>()
               .HasIndex(b => b.Name);

            modelBuilder.Entity<SiteEntity>()
               .HasIndex(b => b.Url);

            modelBuilder.Entity<ProbeEntity>()
               .HasIndex(b => b.SourceIpAddress);

            modelBuilder.Entity<ProbeEntity>()
               .HasIndex(b => b.DestinationIpAddress);

            //modelBuilder.Entity<ConsoleMessageEntity>()
            //   .HasIndex(b => b.Type);

            //modelBuilder.Entity<ConsoleMessageEntity>()
            //   .HasIndex(b => b.Text);


            // Sets the default SQL Server string column length to be 900 characters.
            //IEnumerable<IMutableProperty> allStringCollumns = modelBuilder.Model.GetEntityTypes()
            //        .SelectMany(t => t.GetProperties())
            //        .Where(p => p.ClrType == typeof(string));

            //foreach (IMutableProperty property in allStringCollumns)
            //{
            //    if (property.GetMaxLength() == null)
            //    {
            //        property.SetMaxLength(4000);
            //    }
            //}

            base.OnModelCreating(modelBuilder);
        }
    }
}
