using Npgsql;

namespace MoviePro.Services
{
    public class ConnectionService
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // Updated for AWS
            var databaseUrl = Environment.GetEnvironmentVariable("RDS_HOSTNAME");

            return string.IsNullOrEmpty(databaseUrl) ? connectionString : BuildConnectionString(databaseUrl);
        }

        // Updated for AWS
        private static string BuildConnectionString(string databaseUrl)
        {
            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = Environment.GetEnvironmentVariable("RDS_HOSTNAME"),
                Port = Int32.Parse(Environment.GetEnvironmentVariable("RDS_PORT")),
                Username = Environment.GetEnvironmentVariable("RDS_USERNAME"),
                Password = Environment.GetEnvironmentVariable("RDS_PASSWORD"),
                Database = Environment.GetEnvironmentVariable("RDS_DB_NAME"),

                SslMode = SslMode.Require,
                TrustServerCertificate = true
            };

            return builder.ToString();
        }
    }
}
