using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Scoped Services
                //DBInit.SeedDB(app); // denne må fjernes dersom vi vil beholde dataene i databasen og ikke initialisere 
            }

            app.UseRouting();

            // HTML (wwwroot)
            app.UseStaticFiles();

            // Controllers
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
