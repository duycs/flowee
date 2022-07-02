using CustomerAPI.SeedData;
using CustomerApplication.MappingConfigurations;
using CustomerCrossCutting.DependencyInjections;
using CustomerInfrastructure;
using MediatR;
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
    var customerContext = serviceScope.ServiceProvider.GetRequiredService<CustomerContext>();
    var logger = serviceScope.ServiceProvider.GetService<ILogger<CustomerContextSeed>>();
    var customerContextSeed = new CustomerContextSeed(customerContext, logger, builder.Environment, configuration, builder.Environment.ContentRootPath, "SeedData");
    customerContextSeed.Created();
    customerContextSeed.Migrate();
    customerContextSeed.SeedAsync().Wait();
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
