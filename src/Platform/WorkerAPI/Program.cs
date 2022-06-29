using AppShareServices.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;
using WorkerAPI.SeedData;
using WorkerApplication.MappingConfigurations;
using WorkerCrossCutting.DependencyInjections;
using WorkerInfrastructure.DataAccess;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration;

var configurationBuilder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory());

if (builder.Environment.IsDevelopment())
{
    configurationBuilder.AddJsonFile("appsettings.Development.json", true, true)
    .AddEnvironmentVariables();
}
else if (builder.Environment.IsStaging())
{
    configurationBuilder.AddJsonFile("appsettings.Staging.json", true, true)
    .AddEnvironmentVariables();
}
else if (builder.Environment.IsProduction())
{
    configurationBuilder.AddJsonFile("appsettings.Production.json", true, true)
    .AddEnvironmentVariables();
}

// Add services to the container.
// Cross cutting IoC layers
configuration = configurationBuilder.Build();

// TODO: move to infrastructure
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(AutoMapping));

builder.Services.AddLayersInjector(configuration);

// Ignore object depth is larger than the maximum allowed depth of 32: https://gavilan.blog/2021/05/19/fixing-the-error-a-possible-object-cycle-was-detected-in-different-versions-of-asp-net-core/
builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

// Migrate and seed database
using (var serviceScope = app.Services.CreateScope())
{
    var workerContext = serviceScope.ServiceProvider.GetRequiredService<WorkerContext>();
    var logger = serviceScope.ServiceProvider.GetService<ILogger<WorkerContextSeed>>();
    var workerContextSeed = new WorkerContextSeed(workerContext, logger, builder.Environment, configuration);
    workerContextSeed.Created();
    workerContextSeed.Migrate();
    workerContextSeed.SeedAsync().Wait();

    var eventContext = serviceScope.ServiceProvider.GetRequiredService<EventContext>();
    eventContext.Database.EnsureCreated();
    eventContext.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
