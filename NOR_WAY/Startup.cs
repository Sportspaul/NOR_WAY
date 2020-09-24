using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;

namespace NOR_WAY
{
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
            services.AddScoped<IBussRepository, BussRepository>();
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

            // HTML (wwwroot)
            app.UseStaticFiles();

            // Controllers
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}