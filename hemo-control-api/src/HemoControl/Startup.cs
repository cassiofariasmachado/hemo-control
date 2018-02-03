using HemoControl.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HemoControl
{
    public class Startup
    {

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddSingleton<IConfiguration>(this.Configuration);

            services.AddDbContext<HemoControlContext>(options =>
               options.UseSqlServer(this.Configuration.GetConnectionString("HemoControl"))
            );

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, HemoControlContext context)
        {
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                DbInitializer.Initialize(context);
            }

            app.UseCors(builder =>
            {
                var config = env.IsProduction() ? builder.WithOrigins("*.hemocontrol.com.br") : builder.AllowAnyOrigin();

                config.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });

            app.UseMvc();
        }
    }
}