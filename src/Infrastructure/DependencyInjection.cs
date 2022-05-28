// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CleanArchitecture.Blazor.Infrastructure.Services.Authentication;
using Microsoft.AspNetCore.Components.Server.Circuits;
using HashidsNet;
using CleanArchitecture.Blazor.Infrastructure.Hubs;

namespace CleanArchitecture.Blazor.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("BlazorDashboardDb");
                options.EnableSensitiveDataLogging();
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options => {
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                 options.EnableSensitiveDataLogging();
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
        }
        services.AddSingleton(sp => new Hashids("Blazor.net"));
        services.Configure<DashbordSettings>(configuration.GetSection(DashbordSettings.SectionName));
        services.AddSingleton(s => s.GetRequiredService<IOptions<DashbordSettings>>().Value);
        services.AddScoped<IDbContextFactory<ApplicationDbContext>,BlazorContextFactory<ApplicationDbContext>>();
        services.AddTransient<IApplicationDbContext>(provider => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddTransient<IDomainEventService, DomainEventService>();

        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
       

        services.AddScoped<ProfileService>();
        services.AddScoped<IdentityAuthenticationService>();
        services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<IdentityAuthenticationService>());
      
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IDateTime, DateTimeService>();
        services.AddScoped<IExcelService, ExcelService>();
        services.AddScoped<IUploadService, UploadService>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.Configure<AppConfigurationSettings>(configuration.GetSection(AppConfigurationSettings.SectionName));

        var mailSettings = new MailSettings();
        configuration.GetSection(MailSettings.SectionName).Bind(mailSettings);
        
        services.Configure<MailSettings>(configuration.GetSection(MailSettings.SectionName));
        services.AddSingleton(mailSettings);        
        services.AddScoped<IMailService, SMTPMailService>();
        services.AddAuthentication();
        services.Configure<IdentityOptions>(options =>
        {
            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            // Default Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
            // Here I stored necessary permissions/roles in a constant
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                {
                    options.AddPolicy(propertyValue.ToString(), policy => policy.RequireClaim(ApplicationClaimTypes.Permission, propertyValue.ToString()));
                }
            }
        });
        services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsIdentityFactory>();
        // Localization
        services.AddLocalization(options => options.ResourcesPath = LocalizationConstants.ResourcesPath);
        services.AddScoped<LocalizationCookiesMiddleware>();
        services.AddScoped<ExceptionHandlingMiddleware>();
        services.Configure<RequestLocalizationOptions>(options =>
        {
            options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
            options.AddSupportedCultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
            options.FallBackToParentUICultures = true;

        });
        // configure your sender and template choices with dependency injection.

        services.AddFluentEmail(mailSettings.From)
                .AddRazorRenderer()
        .AddSmtpSender(new System.Net.Mail.SmtpClient()
        {
            Host = mailSettings.Host,
            Port = mailSettings.Port,
            EnableSsl = mailSettings.UseSsl,
            Credentials = new System.Net.NetworkCredential(mailSettings.UserName, mailSettings.Password)
        });
        //.AddMailKitSender(new FluentEmail.MailKitSmtp.SmtpClientOptions()
        //{
        //    RequiresAuthentication = !string.IsNullOrEmpty(mailSettings.UserName),
        //    UsePickupDirectory = false,
        //    Password = mailSettings.Password,
        //    Port = mailSettings.Port,
        //    Server = mailSettings.Host,
        //    User = mailSettings.UserName,
        //    UseSsl = mailSettings.UseSsl,
        //});
 
        services.AddControllers();
        services.AddSingleton<IUsersStateContainer, UsersStateContainer>();
        services.AddScoped<CircuitHandler, CircuitHandlerService>();
        services.AddScoped<HubClient>();
        services.AddSignalR();

        return services;
    }



}
