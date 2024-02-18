using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Contexts
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            _configuration = configuration;
        }

        public DbSet<MeetingEvent> MeetingEvents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            var connectionString = _configuration.GetConnectionString("PostgreSQLConnection");
            options.UseNpgsql(connectionString);

        }
    }
}
