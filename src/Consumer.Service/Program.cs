using Consumer.Service;
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
        // Add Dependencies
        services.Configure<FileRepositorySettings>(builder.Configuration.GetSection("FileRepository"));
        services.AddSingleton<IFileRepositorySettings>(sp => sp.GetRequiredService<IOptions<FileRepositorySettings>>().Value);
        services.AddSingleton<IFileRepository, FileRepository>();

        services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
        services.AddSingleton<IMessageBrokerSettings>(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
        services.AddSingleton<IMessageBroker, RabbitMQBroker>();

        // Add Consumer Background Service
        services.AddHostedService<ConsumerBackgroundService>();
    })
    .Build()
    .Run();