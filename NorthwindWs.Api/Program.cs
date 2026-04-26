using Microsoft.EntityFrameworkCore;
using NorthwindWs.Data;
using NorthwindWs.Services;
using NorthwindWs.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<NorthwindDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnection")));

builder.Services.AddScoped<ICustomerService, CustomerService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<NorthwindDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Map Health Check endpoint to /actuator
app.MapHealthChecks("/actuator");

app.Run();
