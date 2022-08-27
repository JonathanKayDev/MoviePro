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
            var key = _appSettings.MovieProSettings.TmDbApiKey;
            var ev = Environment.GetEnvironmentVariable("TmDbApiKey");

            return string.IsNullOrEmpty(ev) ? key : ev;
        }

        public string? GetDefaultEmail()
        {
            var email = _appSettings.MovieProSettings.DefaultCredentials.Email;
            var ev = Environment.GetEnvironmentVariable("DefaultEmail");

            return string.IsNullOrEmpty(ev) ? email : ev;
        }

            public string? GetDefaultPassword()
        {
            var pw = _appSettings.MovieProSettings.DefaultCredentials.Password;
            var ev = Environment.GetEnvironmentVariable("DefaultPassword");

            return string.IsNullOrEmpty(ev) ? pw : ev;
        }

        public string? GetDemoEmail()
        {
            var email = _appSettings.MovieProSettings.DemoAdminCredentials.Email;
            var ev = Environment.GetEnvironmentVariable("DemoEmail");

            return string.IsNullOrEmpty(ev) ? email : ev;
        }

        public string? GetDemoPassword()
        {
            var pw = _appSettings.MovieProSettings.DemoAdminCredentials.Password;
            var ev = Environment.GetEnvironmentVariable("DemoPassword");

            return string.IsNullOrEmpty(ev) ? pw : ev;
        }

    }
}
