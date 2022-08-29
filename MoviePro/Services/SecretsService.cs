using Microsoft.Extensions.Options;
using MoviePro.Models.Settings;
using MoviePro.Services.Interfaces;

namespace MoviePro.Services
{
    public class SecretsService : ISecretsService
    {
        // Environment variables for AWS
        private readonly AppSettings _appSettings;

        public SecretsService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public string? GetTmDbApiKey()
        {
            var ev = Environment.GetEnvironmentVariable("TmDbApiKey");

            return string.IsNullOrEmpty(ev) ? _appSettings.MovieProSettings.TmDbApiKey : ev;
        }

        public string? GetDefaultEmail()
        {
            var ev = Environment.GetEnvironmentVariable("DefaultEmail");

            return string.IsNullOrEmpty(ev) ? _appSettings.MovieProSettings.DefaultCredentials.Email : ev;
        }

        public string? GetDefaultPassword()
        {
            var ev = Environment.GetEnvironmentVariable("DefaultPassword");

            return string.IsNullOrEmpty(ev) ? _appSettings.MovieProSettings.DefaultCredentials.Password : ev;
        }

        public string? GetDemoEmail()
        {
            var ev = Environment.GetEnvironmentVariable("DemoEmail");

            return string.IsNullOrEmpty(ev) ? _appSettings.MovieProSettings.DemoAdminCredentials.Email : ev;
        }

        public string? GetDemoPassword()
        {
            var ev = Environment.GetEnvironmentVariable("DemoPassword");

            return string.IsNullOrEmpty(ev) ? _appSettings.MovieProSettings.DemoAdminCredentials.Password : ev;
        }

    }
}
