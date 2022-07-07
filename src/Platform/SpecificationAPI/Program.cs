using MediatR;
using SpecificationAPI.SeedData;
using SpecificationApplication.MappingConfigurations;
using SpecificationCrossCutting.DependencyInjections;
using SpecificationInfrastructure.DataAccess;
using System.Reflection;
using System.Text.Json.Serialization;

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

configuration = configurationBuilder.Build();

// Add services to the container.
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(AutoMapping));

builder.Services.AddLayersInjector(configuration);

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Migrate and seed database
using (var serviceScope = app.Services.CreateScope())
{
    var specificationContext = serviceScope.ServiceProvider.GetRequiredService<SpecificationContext>();
    var logger = serviceScope.ServiceProvider.GetService<ILogger<SpecificationContextSeed>>();
    var productContextSeed = new SpecificationContextSeed(specificationContext, builder.Environment.ContentRootPath, "SeedData");
    productContextSeed.Created();
    productContextSeed.Migrate();
    productContextSeed.SeedAsync().Wait();
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
