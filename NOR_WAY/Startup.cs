using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.DAL.Repositories;

namespace NOR_WAY
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Controllers
            services.AddControllers();

            // SQLite DB
            services.AddDbContext<BussContext>(options =>
                            options.UseSqlite("Data Source=Buss.db"));

            // Scoped Services
            services.AddScoped<IAvgangRepository, AvgangRepository>();
            services.AddScoped<IBillettyperRepository, BillettyperRepository>();
            services.AddScoped<IBrukereRepository, BrukereRepository>();
            services.AddScoped<IOrdreRepository, OrdreRepository>();
            services.AddScoped<IRuterRepository, RuterRepository>();
            services.AddScoped<IRuteStoppRepository, RuteStoppRepository>();
            services.AddScoped<IStoppRepository, StoppRepository>();

            // SESSION
            services.AddSession(options =>
            {
                options.Cookie.Name = ".AdventureWorks.Session";
                // Session times ut hvis bruker er Idle i 30 min
                options.IdleTimeout = TimeSpan.FromSeconds(1800);
                options.Cookie.IsEssential = true;
            });
            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                loggerFactory.AddFile("Logs/BussLog.txt");
                // Scoped Services
                DBInit.SeedDB(app); // denne må fjernes dersom vi vil beholde dataene i databasen og ikke initialisere
            }

            app.UseRouting();

            // SESSION
            app.UseSession();

            // HTML (wwwroot)
            app.UseStaticFiles();

            // Controllers
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}