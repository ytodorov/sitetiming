using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace PlaywrightTestLinuxContainer
{
    public class SiteTimingContext : DbContext
    {
        public DbSet<SiteEntity>? Sites { get; set; }
        public DbSet<ProbeEntity>? Probes { get; set; }
        public DbSet<RequestEntity>? Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = @"Server=tcp:hximbq2jrp.database.windows.net,1433;Initial Catalog=websitetimings;Persist Security Info=False;User ID=aYordan;Password=123Pass!@#;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
//#if (DEBUG)
//            connectionString = "Data Source=.\\SQLDEVELOPER2019;Initial Catalog=websitetimings;Integrated Security=True;MultipleActiveResultSets=true;";
//#endif
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
