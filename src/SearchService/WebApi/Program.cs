using FastEndpoints;
using MongoDB.Driver;
using MongoDB.Entities;
using WebApi.Data;
using WebApi.Models;
using WebApi.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ISearchRepository, SearchRepository>();
builder.Services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();
await DatabaseInitializer.InitDb(app);

app.Run();


