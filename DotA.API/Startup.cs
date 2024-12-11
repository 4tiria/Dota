using Dota.API.Models;
using Dota.API.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Dota.Common;
using Dota.API.RabbitMQ;
using Domain.Mongo.API;
using Domain.Mongo.API.Migration;
using Domain.Mongo.API.Seeds;
using Domain.Mongo.API.Repositories.NewsRepository;
using Dota.API.Hero.RabbitMQ.Consumers;
using Dota.API.Hero.RabbitMQ.Producers;

namespace Dota.API;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AppMappingProfile));
        AddAuthentication(services);
        services
            .AddMvc()
            .AddNewtonsoftJson(opts => opts.SerializerSettings
                .Converters
                .Add(new StringEnumConverter())
            );

        services
            .AddControllers()
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

        services.AddCors(x => x.AddPolicy("CorsPolicy",
            options => options
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

        services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDB"));

        services
            .AddNoSql(_configuration)
            .AddSingleton<IHeroProducerService, HeroProducerService>()
            .AddSingleton<IHeroConsumerService, HeroConsumerService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEnumerable<ISeed> seeds)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors("CorsPolicy");
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

        foreach (var seed in seeds)
        {
            seed.SeedData();
        }
    }

    private void AddAuthentication(IServiceCollection services)
    {
        var authOptionsConfiguration = _configuration.GetSection("Auth");
        var authOptions = authOptionsConfiguration.Get<AuthOptions>();
        services.Configure<AuthOptions>(authOptionsConfiguration);

        var controllerValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions!.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true,
        };

        var validationForRefreshTokenParameters = new TokenValidationParameters()
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