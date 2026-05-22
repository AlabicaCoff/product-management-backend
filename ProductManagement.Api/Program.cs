using ProductManagement.Api.Configurations;
using ProductManagement.Api.Utilities.Seeders;
using ProductManagement.Api.Utilities.Seeders.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Api.Data;
using ProductManagement.Api.Models;
using ProductManagement.Api.Services;
using ProductManagement.Api.Services.Interfaces;
using ProductManagement.Api.Repositories;
using ProductManagement.Api.Repositories.Interfaces;
using ProductManagement.Api.Utilities.Helper.AutoMapper;

namespace ProductManagement.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(mc =>
            {
                mc.AddProfile<CategoryProfile>();
                mc.AddProfile<ProductProfile>();
            });

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
                    var configureTokenValidation = TokenValidationConfiguration.Configure(builder);
                    configureTokenValidation(options.TokenValidationParameters);
                });

            builder.Services.AddCors(CorsConfiguration.Configure());

            // Seeders
            builder.Services.AddScoped<ISeeder, RolesSeeder>();
            builder.Services.AddScoped<ISeeder, UsersSeeder>();
            builder.Services.AddScoped<ISeeder, UserRolesSeeder>();
            builder.Services.AddScoped<ApplicationSeeder>();

            // Repositories
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            // Services
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();

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
        }
    }
}
