using Application;
using Domain.ApplicationUser;
using FluentValidation;
using Infrastructure;
using Serilog;
using Web.Pages.Account.Register;
using Web.Services;

namespace Web
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddValidatorsFromAssemblyContaining<InputModelValidator>();
            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddRazorPages();

            builder.Services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.IssuerUri = builder.Configuration["IssuerUrl"];
                    // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                    //options.EmitStaticAudienceClaim = true;
                })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients(builder.Configuration))
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CarstiesProfileService>()
                .AddPersistedGrantStore<CarstiesPersistedGrantStore>();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
            });

            builder.Services.AddAuthentication();

            return builder.Build();
        }

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.MapRazorPages()
                .RequireAuthorization();

            return app;
        }
    }
}