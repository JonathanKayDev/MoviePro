using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MoviePro.Data;
using MoviePro.Models.Database;
using MoviePro.Models.Settings;

namespace MoviePro.Services
{
    public class SeedService
    {
        private readonly AppSettings _appSettings;
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SecretsService _secretsService;

        public SeedService(IOptions<AppSettings> appSettings, ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SecretsService secretsService)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _secretsService = secretsService;
        }

        public async Task ManageDataAsync()
        {
            await UpdateDatabaseAsync();
            await SeedRolesAsync();
            await SeedUsersAsync();
            await SeedCollections();
        }

        private async Task UpdateDatabaseAsync()
        {
            await _dbContext.Database.MigrateAsync();
        }

        private async Task SeedRolesAsync()
        {
            if (_dbContext.Roles.Any())
            {
                return;
            }

            // Create adminitrator role
            var adminRole = _appSettings.MovieProSettings.DefaultCredentials.Role;
            await _roleManager.CreateAsync(new IdentityRole(adminRole));

            var demoAdminRole = _appSettings.MovieProSettings.DemoAdminCredentials.Role;
            await _roleManager.CreateAsync(new IdentityRole(demoAdminRole));
        }

        private async Task SeedUsersAsync()
        {
            if (_userManager.Users.Any())
            {
                return;
            }

            // Admin
            var newUser = new IdentityUser()
            {
                Email = _secretsService.GetDefaultEmail(),
                UserName = _secretsService.GetDefaultEmail(),
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(newUser, _secretsService.GetDefaultPassword());
            await _userManager.AddToRoleAsync(newUser, _appSettings.MovieProSettings.DefaultCredentials.Role);

            // Demo Admin
            var newDemoUser = new IdentityUser()
            {
                Email = _secretsService.GetDemoEmail(),
                UserName = _secretsService.GetDemoEmail(),
                EmailConfirmed = true
            };

            await _userManager.CreateAsync(newDemoUser, _secretsService.GetDemoPassword());
            await _userManager.AddToRoleAsync(newDemoUser, _appSettings.MovieProSettings.DemoAdminCredentials.Role);
        }

        private async Task SeedCollections()
        {
            if (_dbContext.Collection.Any())
            {
                return;
            }

            _dbContext.Add(new Collection()
            {
                Name = _appSettings.MovieProSettings.DefaultCollection.Name,
                Description = _appSettings.MovieProSettings.DefaultCollection.Description
            });

            await _dbContext.SaveChangesAsync();
        }

    }
}
