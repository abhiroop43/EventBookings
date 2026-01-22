using BuildingBlocks.Behaviors;
using Carter;
using Lookups.Api.Data;
using Lookups.Api.Data.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCarter();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

var connectionString = builder.Configuration.GetConnectionString("Database");
var databaseName = builder.Configuration.GetValue<string>("DatabaseName");
var mongoClient = new MongoClient(connectionString);

builder.Services.AddDbContext<LookupsDbContext>(options => options.UseMongoDB(mongoClient, databaseName!));

builder.Services.AddScoped<ILookupRepository, LookupRepository>();
builder.Services.Decorate<ILookupRepository, CachedLookupRepository>();

var app = builder.Build();

app.MapCarter();
app.UseExceptionHandler(opts => { });

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

await app.RunAsync();