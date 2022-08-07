using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using SatellEarthAPI.Application.Common.Interfaces;
using SatellEarthAPI.Infrastructure.Files;
using SatellEarthAPI.Infrastructure.Identity;
using SatellEarthAPI.Infrastructure.Persistence;
using SatellEarthAPI.Infrastructure.Persistence.Interceptors;
using SatellEarthAPI.Infrastructure.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("SatellEarthAPIDb"));
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                    builder => builder
                    .UseNetTopologySuite()
                    .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            //options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
            //    builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<ApplicationDbContextInitialiser>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddIdentityServer()
            .AddApiAuthorization<ApplicationUser, ApplicationDbContext>();

        services.AddTransient<IDateTime, DateTimeService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();

        services.AddAuthentication()
            .AddIdentityServerJwt();

        services.AddAuthorization(options =>
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator")));

        return services;
    }
}