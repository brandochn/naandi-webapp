using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Naandi.Shared.Services;
using WebApi.Services;
using Microsoft.OpenApi.Models;
using Naandi.Shared.DataBase;
using WebApi.Middleware;

namespace WebApi
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Naandi API", Version = "v1" });
            });
            services.AddTokenAuthentication(Configuration);

            services.AddTransient(_ => new ApplicationDbContext(Configuration["ConnectionString"]));

            services.AddScoped<IRegistrationRequest, RegistrationRequestRepository>();
            services.AddScoped<IFamilyResearch, FamilyResearchRepository>();
            services.AddScoped<IJwt, JwtRepository>();
            services.AddScoped<IUser, UserRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api for Naandi foundation");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
