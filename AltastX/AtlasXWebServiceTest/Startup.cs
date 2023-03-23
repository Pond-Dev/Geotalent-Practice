using AtlasX.Engine.AgsResourceProxy;
using AtlasX.Engine.AgsResourceProxy.Services;
using atlastX.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace atlastX
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
            // For test only, don't do this on production.
            // services.AddCors(c => c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin()));

            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp";
            });

            services.Configure<AppSettings>(Configuration.GetSection("WebServiceSettings"));

            // Setting up proxy config.
            services.AddAgsProxyServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSettings> appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // For test only, don't do this on production.
            // app.UseCors(options => options.AllowAnyOrigin());

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseDataParser(appSettings.Value.General.WebServiceEndpoint, appSettings.Value.DataParser.Paths);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            // Setting up proxy page.
            app.UseWhen(context =>
            {
                return context.Request.Path.Value.ToLower().StartsWith("/api/AppProxy", StringComparison.OrdinalIgnoreCase);
                //&& context.User.Identity.IsAuthenticated; // Add this back in to keep unauthenticated users from utilzing the proxy.
            }, builder => builder.UseAgsProxyServer(
                app.ApplicationServices.GetService<IProxyConfigService>(),
                app.ApplicationServices.GetService<IProxyService>(),
                app.ApplicationServices.GetService<IMemoryCache>()
            ));

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "../ClientApp";

                if (env.IsDevelopment())
                {
                    // Uncomment to improve serve between .NET Core and Angular.
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }
    }
}
