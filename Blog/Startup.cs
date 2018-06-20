using Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;

namespace Blog
{
    public class Startup
    {
        public static readonly LoggerFactory _loggerFactory
            = new LoggerFactory(new[]
            {
                new ConsoleLoggerProvider((category, level) =>
                category == DbLoggerCategory.Database.Command.Name && level == LogLevel.Information, true)
            });

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogContext>(options => options
                .EnableSensitiveDataLogging()
                .UseLoggerFactory(_loggerFactory)
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Blog")));


            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddMvc();
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(14);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "root",
                    template: "{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );

                routes.MapRoute(
                    name: "account",
                    template: "{action}/{id?}",
                    defaults: new { controller = "Account" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}
