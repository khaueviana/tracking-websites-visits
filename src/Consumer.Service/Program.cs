using Application.Services;
using Application.Services.Interfaces;
using Consumer.Service;
using Domain.Model.Interfaces;
using Gateway.Data.File;
using Gateway.Messaging.RabbitMQ;
using Infrastructure.CrossCutting.Interfaces;
using Infrastructure.CrossCutting.Settings;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

Host.CreateDefaultBuilder()
    .ConfigureServices((builder, services) =>
    {
        // Add File Repository Dependency
        services.Configure<FileRepositorySettings>(builder.Configuration.GetSection("FileRepository"));
        services.AddSingleton<IFileRepositorySettings>(sp => sp.GetRequiredService<IOptions<FileRepositorySettings>>().Value);
        services.AddSingleton<ITrackingRepository, FileRepository>();

        // Add Message Broker Dependency
        services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
        services.AddSingleton<IMessageBrokerSettings>(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        services.AddSingleton<IMessageBroker, RabbitMQBroker>();

        // Add Application Services Dependency
        services.AddSingleton<ITrackingApplicationService, TrackingApplicationService>();

        // Add Consumer Background Service
        services.AddHostedService<ConsumerBackgroundService>();
    })
    .Build()
    .Run();