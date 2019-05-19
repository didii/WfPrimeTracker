using System;
using Hangfire;
using Hangfire.Console;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WfPrimeTracker.Business;
using WfPrimeTracker.Business.Jobs;
using WfPrimeTracker.Server.HangfireHelpers;

namespace WfPrimeTracker.Server {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHangfire(conf => {
                conf.UseSqlServerStorage(Configuration.GetConnectionString("Default"));
                conf.UseConsole();
            });

            services.AddBusinessServices(Configuration);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                });
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseHangfireDashboard("/hangfire",
                                     new DashboardOptions() {
                                         Authorization = new[] { new HangfireAutorizationFilter() }
                                     });
            app.UseHangfireServer();

            // Set recurring job to sync data daily
            RecurringJob.AddOrUpdate<IFullScraperJob>(job => job.Invoke(null), Cron.Daily(4, 0));
            // Add job as late as possible for a full reset when necessary
            BackgroundJob.Schedule<IResetDataJob>(job => job.Invoke(null), DateTimeOffset.MaxValue);

            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}