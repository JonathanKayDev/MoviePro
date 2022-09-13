using Npgsql;

namespace MoviePro.Services
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Updated for AWS
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        // Updated for AWS
        private static string BuildConnectionString(string databaseUrl)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = Environment.GetEnvironmentVariable("PGHOST"),
                Port = Int32.Parse(Environment.GetEnvironmentVariable("PGPORT")),
                Username = Environment.GetEnvironmentVariable("PGUSER"),
                Password = Environment.GetEnvironmentVariable("PGPASSWORD"),
                Database = Environment.GetEnvironmentVariable("PGDATABASE"),

                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
    }
}
