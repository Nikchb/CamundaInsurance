using Camunda.Worker;
using Camunda.Worker.Client;
using CamundaInsurance.Data;
using CamundaInsurance.Data.Models;
using CamundaInsurance.Handlers;
using CamundaInsurance.Services;
using CamundaInsurance.Services.Camunda;
using CamundaInsurance.Services.Insurance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CamundaInsurance
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDB(services);
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();
            services.AddTransient<IdentityService>();
            services.AddTransient<InsuranceManager>();
            services.AddSingleton(new CamundaProcessStarter());
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddExternalTaskClient()
                .ConfigureHttpClient((provider, client) =>
                {
                    client.BaseAddress = new Uri($"http://{Environment.GetEnvironmentVariable("CAMUNDA_URL") ?? "localhost:8080"}/engine-rest");
                });

            services.AddCamundaWorker("sampleWorker")
                .AddHandler<InsuranceDenyHandler>()
                .AddHandler<InsuranceApproveHandler>();
        }

        private void ConfigureDB(IServiceCollection services)
        {
            var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
            var user = Environment.GetEnvironmentVariable("DB_USERNAME") ?? "camunda";
            var password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "camunda";                    
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql($"Host={host};Port={port};Database=webapp;Username={user};Password={password}"),
                ServiceLifetime.Transient);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext context)
        {
            CamundaStartup.ConfigureCamunda().Wait();  
            
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/Blazor/Shared/_Host");
            });
        }
    }
}
