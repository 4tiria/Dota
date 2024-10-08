using System;
using DataAccess;
using DotA.API.Models;
using DataAccess.Seeds;
using DotA.API.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;

namespace DotA.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AppMappingProfile));
            AddAuthentication(services);
            services.AddMvc()
                .AddNewtonsoftJson(opts => opts.SerializerSettings
                    .Converters
                    .Add(new StringEnumConverter())
                );
            
            services.AddControllers().AddNewtonsoftJson(x =>
                x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddCors(x => x.AddPolicy("CorsPolicy",
                options => options
                    .SetIsOriginAllowed(_ => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()));

            services.AddDbContext<ApiContext>(options =>
            {
                options.EnableSensitiveDataLogging();
                options.UseMySQL(
                    Configuration["Data:ConnectionString"]);
            });

            services.AddTransient<HeroSeed>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, HeroSeed seed)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors("CorsPolicy");
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "api/{controller}/{action}/{id?}");
            });
            seed.SeedData();
        }

        private void AddAuthentication(IServiceCollection services)
        {
            var authOptionsConfiguration = Configuration.GetSection("Auth");
            var authOptions = authOptionsConfiguration.Get<AuthOptions>();
            services.Configure<AuthOptions>(authOptionsConfiguration);

            var controllerValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = authOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = authOptions.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
            
            var validationForRefreshTokenParameters =  new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = authOptions.Issuer,

                ValidateAudience = true,
                ValidAudience = authOptions.Audience,

                ValidateLifetime = false,

                IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true,
            };
            
            
            services.AddSingleton(validationForRefreshTokenParameters);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = controllerValidationParameters;
                });
        }
    }
}