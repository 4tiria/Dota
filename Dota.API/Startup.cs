using Domain.Mongo.API;
using Domain.Mongo.API.Mappers;
using Dota.API.Hero.RabbitMq;
using Dota.API.Hero.RabbitMq.Consumers;
using Dota.API.Hero.RabbitMq.DLX;
using Dota.API.Hero.RabbitMq.Producers;
using Dota.API.Mappers;
using Dota.API.Models;
using Dota.API.RabbitMQ;
using Dota.API.Statistics.RabbitMq.DLX;
using Dota.API.Statistics.RabbitMq.Producers;
using Dota.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson.Serialization.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using RabbitMQ.Client;

namespace Dota.API;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddAutoMapper(
                typeof(AppMappingProfile), 
                typeof(SeedHeroProfile));

        AddAuthentication(services);
        services
            .AddMvc()
            .AddNewtonsoftJson(opts => opts.SerializerSettings
                .Converters
                .Add(new StringEnumConverter())
            );

        services
            .AddControllers()
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddCors(x => x.AddPolicy("CorsPolicy",
            options => options
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

        services.Configure<MongoDbSettings>(_configuration.GetSection("MongoDB"));

        var conventionPack = new ConventionPack
        {
            new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
        };
        ConventionRegistry.Register("EnumAsString", conventionPack, type => type.IsEnum);
        
        services
            .AddNoSql(_configuration)
            .AddSingleton<IHeroProducerService, HeroProducerService>()
            .AddSingleton<IHeroConsumerService, HeroConsumerService>()
            .AddSingleton<IHeroDlxService, HeroDlxService>()
            .AddSingleton<IStatisticsDlxService, StatisticsDlxService>()
            .AddSingleton<IStatisticsProducerService, StatisticsProducerService>();

        var rabbitMQSettings = configuration.GetSection("RabbitMQ");
        services
            .AddSingleton<IConnectionFactory>(serviceProvider =>
            {
                return new ConnectionFactory
                {
                    HostName = rabbitMQSettings["HostName"],
                    UserName = rabbitMQSettings["UserName"],
                    Password = rabbitMQSettings["Password"]
                };
            })
            .AddSingleton<IConnection>(serviceProvider =>
                serviceProvider.GetRequiredService<IConnectionFactory>().CreateConnection())
            .AddSingleton<IModel>(
                serviceProvider => serviceProvider.GetRequiredService<IConnection>().CreateModel());

        services.AddHostedService<HeroBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEnumerable<ISeed> seeds)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, _configuration["Assets:RelativePath"]!))),
            RequestPath = "/assets"
        });
        
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseCors("CorsPolicy");
        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                "default",
                "api/{controller}/{action}/{id?}");
        });

        foreach (var seed in seeds) seed.SeedData();
    }

    private void AddAuthentication(IServiceCollection services)
    {
        var authOptionsConfiguration = _configuration.GetSection("Auth");
        var authOptions = authOptionsConfiguration.Get<AuthOptions>();
        services.Configure<AuthOptions>(authOptionsConfiguration);

        var controllerValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions!.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };

        var validationForRefreshTokenParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = authOptions.Issuer,

            ValidateAudience = true,
            ValidAudience = authOptions.Audience,

            ValidateLifetime = false,

            IssuerSigningKey = authOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
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