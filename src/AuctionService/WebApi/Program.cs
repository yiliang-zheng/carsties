using System.Reflection;
using Application;
using FastEndpoints;
using Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);
});
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddFastEndpoints();
builder.Services.AddAutoMapper(
    Assembly.GetAssembly(typeof(Program))
);


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();
app.UseFastEndpoints();

//seed data
if (!app.Environment.IsProduction())
{
    await app.Services.InitializeDatabase();
}
app.Run();


