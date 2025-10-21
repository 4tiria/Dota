using System.Reflection;
using Domain.Mongo.API;
using Domain.Mongo.API.Mappers;
using Dota.API.Account.DTO;
using Dota.API.BackgroundWorkers;
using Dota.API.Common;
using Dota.API.Hero.RabbitMq;
using Dota.API.Hero.RabbitMq.Consumers;
using Dota.API.Hero.RabbitMq.DLX;
using Dota.API.Hero.RabbitMq.Producers;
using Dota.API.Mappers;
using Dota.API.RabbitMQ;
using Dota.API.Statistics.RabbitMq.DLX;
using Dota.API.Statistics.RabbitMq.Producers;
using Dota.API.WebSocket;
using Dota.API.WebSocket.Account;
using MassTransit;
using MediatR;
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
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMediatR(mediatrConfiguration => 
            mediatrConfiguration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
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
        services.AddSignalR();
        
        services.AddCors(x => x.AddPolicy("CorsPolicy",
            options => options
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()));

        services.Configure<MongoDbSettings>(configuration.GetSection("MongoDB"));

        var conventionPack = new ConventionPack
        {
            new EnumRepresentationConvention(MongoDB.Bson.BsonType.String)
        };
        ConventionRegistry.Register("EnumAsString", conventionPack, type => type.IsEnum);
        
        services
            .AddNoSql(configuration)
            .AddSingleton<IHeroProducerService, HeroProducerService>()
            .AddSingleton<IHeroConsumerService, HeroConsumerService>()
            .AddSingleton<IHeroDlxService, HeroDlxService>()
            .AddSingleton<IStatisticsDlxService, StatisticsDlxService>()
            .AddSingleton<IStatisticsProducerService, StatisticsProducerService>();

        var rabbitMqSettings = configuration.GetSection("RabbitMQ");
        services
            .AddSingleton<IConnectionFactory>(_ => new ConnectionFactory
            {
                HostName = rabbitMqSettings["HostName"],
                UserName = rabbitMqSettings["UserName"],
                Password = rabbitMqSettings["Password"]
            })
            .AddSingleton<IConnection>(serviceProvider =>
                serviceProvider.GetRequiredService<IConnectionFactory>().CreateConnection())
            .AddSingleton<IModel>(
                serviceProvider => serviceProvider.GetRequiredService<IConnection>().CreateModel());
        
        services.AddMassTransit(x =>
        {
            x.UsingInMemory(); // чтобы работали временные очереди, без брокера

            x.AddRider(rider =>
            {
                rider.AddProducer<string, AccountCreated>("accounts");

                rider.AddConsumer<AccountsConsumer>();

                rider.UsingKafka((context, k) =>
                {
                    k.Host(configuration["Kafka:Url:localhost"]);

                    k.TopicEndpoint<string, AccountCreated>("accounts", "accounts-group", e =>
                    {
                        e.ConfigureConsumer<AccountsConsumer>(context);
                    });
                });
            });
        });
        
        services.AddHostedService<HeroBackgroundService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(
                Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, configuration["Assets:RelativePath"]!))),
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
            endpoints.MapHub<NotificationHub>("hubs/notifications");
            endpoints.MapHub<AccountFeedHub>("hubs/account/feed");
            endpoints.MapControllerRoute(
                "default",
                "api/{controller}/{action}/{id?}");
        });
    }

    private void AddAuthentication(IServiceCollection services)
    {
        var authOptionsConfiguration = configuration.GetSection("Auth");
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