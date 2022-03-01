using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MoviePro.Data;
using MoviePro.Models.Settings;
using MoviePro.Services;
using MoviePro.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = ConnectionService.GetConnectionString(builder.Configuration);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// register and inject IOptions of type AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
// register SeedService
builder.Services.AddTransient<SeedService>();
// register TMDBMovieService and HttpClient
builder.Services.AddHttpClient();
builder.Services.AddScoped<IRemoteMovieService, TMDBMovieService>();
// register TMDBMappingService
builder.Services.AddScoped<IDataMappingService, TMDBMappingService>();
// register BasicImageService
builder.Services.AddSingleton<IImageService, BasicImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

//
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// call SeedService, this must be after setswitch above
var dataService = app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedService>();
await dataService.ManageDataAsync();

app.Run();
