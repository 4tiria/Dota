using DotA.API.Models;
using DotA.API.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using NoSql;
using Dota.Common;
using NoSql.Seeds;
using CoreModule.Heroes.Repository;
using CoreModule.Matches.Repository;
using NoSql.Repositories.NewsRepository;
using DotA.API.RabbitMQ;

namespace DotA.API;

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

        services.Configure<MongoDbSettings>(Configuration.GetSection("MongoDB"));

        services
            .AddSingleton<MongoDbContext>()
            .AddSingleton<IRabbitMQProducerService, RabbitMQProducerService>()
            .AddTransient<IHeroRepository, HeroRepository>()
            .AddTransient<IMatchRepository, MatchRepository>()
            .AddTransient<INewsRepository, NewsRepository>()
            .AddTransient<ISeed, NewsSeed>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEnumerable<ISeed> seeds)
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

        foreach (var seed in seeds)
        {
            seed.SeedData();
        }
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