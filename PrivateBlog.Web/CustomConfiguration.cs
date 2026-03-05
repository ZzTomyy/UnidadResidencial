using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UnidadResidencial.Web.Data;
using UnidadResidencial.Web.Data.Entities;
using UnidadResidencial.Web.Data.Seeders;
using UnidadResidencial.Web.Helpers.Abstractions;
using UnidadResidencial.Web.Helpers.Implementations;
using UnidadResidencial.Web.Services.Abstractions;
using UnidadResidencial.Web.Services.Abtractions;
using UnidadResidencial.Web.Services.Implementations;

namespace UnidadResidencial.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomConfiguration(this WebApplicationBuilder builder)
        {
            string? cnn = builder.Configuration.GetConnectionString("MyConnection");

            // Data Context
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });

            // AutoMapper
            builder.Services.AddAutoMapper(typeof(Program));

            // Toast Notification Setup
            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            // Identity and Access Management
            AddIAM(builder);

            // Services
            AddServices(builder);

            builder.Services.AddHttpContextAccessor();

            return builder;
        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>()
              .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(100);
                conf.LoginPath = "/Account/Login";
                conf.AccessDeniedPath = "/Error/403";
            });
        }

        private static void AddServices(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ISectionsService, SectionsService>();
            builder.Services.AddScoped<IBlogsService, ResidencialService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddTransient<SeedDb>();

            builder.Services.AddTransient<ICombosHelper, CombosHelper>();
        }

        public static async Task<WebApplication> AddCustomWebApplicationConfigurationAsync(this WebApplication app)
        {
            app.UseNotyf();
            await SeedDataAsync(app);  // ✔️ ahora async
            return app;
        }

        private static async Task SeedDataAsync(WebApplication app)
        {
            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();

            var service = scope.ServiceProvider.GetRequiredService<SeedDb>();
            await service.SeedAsync(); // ✔️ AHORA asynchronous
        }
    }
}