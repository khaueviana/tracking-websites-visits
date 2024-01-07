using Application.Services;
using Application.Services.Interfaces;
using Domain.Model.Interfaces;
using Gateway.Data.File;
using Gateway.Messaging.RabbitMQ;
using Infrastructure.CrossCutting.Interfaces;
using Infrastructure.CrossCutting.Settings;
using Microsoft.Extensions.Options;
using Presentation.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add File Repository Dependency
builder.Services.Configure<FileRepositorySettings>(builder.Configuration.GetSection("FileRepository"));
builder.Services.AddSingleton<IFileRepositorySettings>(sp => sp.GetRequiredService<IOptions<FileRepositorySettings>>().Value);
builder.Services.AddSingleton<ITrackingRepository, FileRepository>();

// Add Message Broker Dependency
builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton<IMessageBrokerSettings>(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddSingleton<IMessageBroker, RabbitMQBroker>();

// Add Application Services Dependency
builder.Services.AddSingleton<ITrackingApplicationService, TrackingApplicationService>();

// Add Swagger Configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map Endpoints
app.MapTrackingEndpoints();

app.Run();