using Gateway.Messaging.RabbitMQ;
using Infrastructure.CrossCutting.Interfaces;
using Infrastructure.CrossCutting.Settings;
using Microsoft.Extensions.Options;
using Presentation.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add Dependencies
builder.Services.Configure<MessageBrokerSettings>(builder.Configuration.GetSection("MessageBroker"));
builder.Services.AddSingleton<IMessageBrokerSettings>(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);
builder.Services.AddSingleton<IMessageBroker, RabbitMQBroker>();

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