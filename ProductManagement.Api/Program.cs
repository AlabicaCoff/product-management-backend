using ProductManagement.Api.Configurations;
using ProductManagement.Api.Utilities;
using ProductManagement.Api.Utilities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Services;
using ProductManagement.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("ManagementUser")
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure(IdentityConfiguration.Configure());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        // TokenValidationConfiguration.Configure(builder) returns an Action<TokenValidationParameters>
        // invoke it to configure the JwtBearerOptions.TokenValidationParameters
        var configureTokenValidation = TokenValidationConfiguration.Configure(builder);
        configureTokenValidation(options.TokenValidationParameters);
    });

builder.Services.AddCors(CorsConfiguration.Configure());

builder.Services.AddScoped<ISeeder, RolesSeeder>();
builder.Services.AddScoped<ISeeder, UsersSeeder>();
builder.Services.AddScoped<ISeeder, UserRolesSeeder>();
builder.Services.AddScoped<ApplicationSeeder>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<ApplicationSeeder>();
    await seeder.SeedAllAsync(db, scope.ServiceProvider);
}

app.Run();
