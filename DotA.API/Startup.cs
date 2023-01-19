using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using DotA.API.Models;
using DataAccess.Seeds;
using DotA.API.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()));
      
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

      app.UseStaticFiles();
      app.UseHttpsRedirection();
      app.UseRouting();
      
      app.UseAuthentication();
      app.UseAuthorization();

      SetupEndpoints(app);
      seed.SeedData();
    }

    private void SetupEndpoints(IApplicationBuilder app)
    {
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "api/{controller}/{action}/{id?}");
      });
    }
    
    private void AddAuthentication(IServiceCollection services)
    {
      var authOptionsConfiguration = Configuration.GetSection("Auth");
      var authOptions = authOptionsConfiguration.Get<AuthOptions>();
      services.Configure<AuthOptions>(authOptionsConfiguration);
      
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
          options.RequireHttpsMetadata = false;
          options.TokenValidationParameters = new TokenValidationParameters()
          {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = true,

            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
          };
        });
    }
  }
}
