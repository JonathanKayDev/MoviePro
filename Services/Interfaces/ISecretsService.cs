namespace MoviePro.Services.Interfaces
{
    public interface ISecretsService
    {
        public string? GetTmDbApiKey();
        public string? GetDefaultEmail();
        public string? GetDefaultPassword();
        public string? GetDemoEmail();
        public string? GetDemoPassword();
    }
}
