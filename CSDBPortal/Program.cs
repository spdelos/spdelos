using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CSDBPortal.Data;
using CSDBPortal.Business;
using CSDBPortal.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Append connection pool settings to the configured connection string
var baseConnection = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = baseConnection + ";Min Pool Size=10;Max Pool Size=300;Connect Timeout=30;";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
    .AddDefaultTokenProviders()
    .AddDefaultUI()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddSingleton<ActiveSessionTracker>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnValidatePrincipal = context =>
    {
        var tracker = context.HttpContext.RequestServices.GetRequiredService<ActiveSessionTracker>();
        var userId = context.Principal?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId != null)
            tracker.Touch(userId);
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization();
builder.Services.AddAuthentication();
builder.Services.AddMvc();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase);

// Register managers as Scoped so they share the request-scoped DbContext
builder.Services.AddScoped<BaseManager>();
builder.Services.AddScoped<ManageManager>();
builder.Services.AddScoped<AdministrationManager>();
builder.Services.AddScoped<ConfigurationsManager>();
builder.Services.AddScoped<ICNManager>();
builder.Services.AddScoped<BrexValidationEngine>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
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

app.Run();
