using Carter;
using Dotnet8_MinimalApi_Carter;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add DI - Add Service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<VehicleDb>(opt => opt.UseInMemoryDatabase("VehicleList"));
builder.Services.AddCarter();

var app = builder.Build();

// Configure Pipeline - Use Method...


app.UseSwagger();
app.UseSwaggerUI();
app.MapCarter();

app.Run();
