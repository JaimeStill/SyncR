using Sync.Hubs;
using SyncR.Server;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

string[] origins = builder
    .Configuration
    .GetSection("CorsOrigins")
    .Get<string[]>()
?? new string[] {
    "http://localhost:4200",
    "http://localhost:5001",
    "http://localhost:5002"
};

builder.Services.AddCors(options =>
    options.AddDefaultPolicy(policy =>
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(origins)
    )
);
builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();
builder.Services.AddSyncServiceManager();

var app = builder.Build();

app.Services.MapSyncServices(new List<SyncService>()
{
    new("Processor", "http://localhost:5001/api/status", typeof(ProcessorHub))
});

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.MapControllers();
app.MapHubs();

app.Run();
