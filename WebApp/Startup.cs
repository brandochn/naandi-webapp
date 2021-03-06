using jsreport.AspNetCore;
using jsreport.Binary;
using jsreport.Local;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Naandi.Shared.DataBase;
using Naandi.Shared.Services;
using System;
using System.Runtime.InteropServices;
using WebApp.Data;
using WebApp.Services;
using WebApp.SessionState;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
            });

            services.AddTransient(_ => new ApplicationRestClient(Configuration["AppServiceUri"]));
            services.AddTransient(_ => new ApplicationDbContext(Configuration["ConnectionString"]));
            services.AddHttpContextAccessor();
            bool IsWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            if (IsWindows == true)
            {
                services.AddJsReport(new LocalReporting().UseBinary(JsReportBinary.GetBinary()).AsUtility().Create());
            }
            else
            {
                services.AddJsReport(new LocalReporting().UseBinary(jsreport.Binary.Linux.JsReportBinary.GetBinary()).AsUtility().Create());
            }

            services.AddControllersWithViews()
                .AddNewtonsoftJson();
            services.AddRazorPages().AddMvcOptions(options =>
            {
                options.MaxModelValidationErrors = 50;
                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                    _ => "The field is required.");
            });

            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.SameSite = SameSiteMode.Strict;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
            });

            services.AddAuthentication("CookieAuth") // Sets the default scheme to cookies
                .AddCookie("CookieAuth", options =>
                {
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.LoginPath = "/Account/Login";
                }).Services.ConfigureApplicationCookie(options =>
                {
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromHours(12);
                });

            services.AddScoped<IRegistrationRequest, RegistrationRequestRepository>();
            services.AddScoped<IFamilyResearch, FamilyResearchRepository>();
            services.AddScoped<IUser, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            UserSession.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>(), Configuration);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "socialwork_registrationrequest",
                    areaName: "SocialWork",
                    pattern: "SocialWork/{controller=RegistrationRequest}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                  name: "default",
                  pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
    }
}