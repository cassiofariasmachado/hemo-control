using System;
using System.IO;
using System.Reflection;
using HemoControl.Database;
using HemoControl.Interfaces.Services;
using HemoControl.Middlewares;
using HemoControl.Services;
using HemoControl.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

namespace HemoControl
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddDbContext<HemoControlContext>(options =>
               options.UseMySql(Configuration.GetConnectionString("HemoControl"))
            );

            var accessTokenSettings = new AccessTokenSettings();
            new ConfigureFromConfigurationOptions<AccessTokenSettings>(
                Configuration.GetSection("AccessToken"))
                    .Configure(accessTokenSettings);
            services.AddSingleton(accessTokenSettings);

            var signingSettings = new SigningSettings();
            services.AddSingleton(signingSettings);

            services.AddSingleton<IPasswordService, PasswordService>();
            services.AddSingleton<IAccessTokenService, AccessTokenService>();

            IdentityModelEventSource.ShowPII = true;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var tokenValidationParameters = options.TokenValidationParameters;

                    tokenValidationParameters.IssuerSigningKey = signingSettings.Key;
                    tokenValidationParameters.ValidAudience = accessTokenSettings.Audience;
                    tokenValidationParameters.ValidIssuer = accessTokenSettings.Issuer;

                    tokenValidationParameters.ValidateIssuerSigningKey = true;
                    tokenValidationParameters.ValidateLifetime = true;

                    tokenValidationParameters.ClockSkew = TimeSpan.Zero;
                });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(
                    JwtBearerDefaults.AuthenticationScheme,
                    new AuthorizationPolicyBuilder()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                        .RequireAuthenticatedUser()
                        .Build()
                );
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "HemoControl API",

                    //To-do: fill this infos
                    // Description = "A simple example ASP.NET Core Web API", 
                    // TermsOfService = new Uri("https://example.com/terms"),
                    // Contact = new OpenApiContact
                    // {
                    //     Name = "Shayne Boyer",
                    //     Email = string.Empty,
                    //     Url = new Uri("https://twitter.com/spboyer"),
                    // },
                    // License = new OpenApiLicense
                    // {
                    //     Name = "Use under LICX",
                    //     Url = new Uri("https://example.com/license"),
                    // }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. Example: 'Bearer {accessToken}'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<ExceptionMiddleware>();

            services.AddHttpContextAccessor();

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HemoControlContext hemoControlContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DbInitializer.Initialize(hemoControlContext);

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HemoControl API V1");
            });

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}