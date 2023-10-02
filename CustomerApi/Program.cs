using Microsoft.EntityFrameworkCore;
using CustomerApi.Data;
using CustomerApi.Models;
using SharedModels;
using System;
using CustomerApi.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
string ConnectionString = "host=goose-01.rmq2.cloudamqp.com;virtualHost=suwoyvyw;username=suwoyvyw;password=MaEUT7-L6AdEM5jLGvtXTIBpLzGwfcLc";

// Add services to the container.

builder.Services.AddDbContext<CustomerApiContext>(opt => opt.UseInMemoryDatabase("CustomersDb"));

// Register repositories for dependency injection
builder.Services.AddScoped<IRepository<Customer>, CustomerRepository>();

// Register database initializer for dependency injection
builder.Services.AddTransient<IDbInitializer, DbInitializer>();

// Register ProductConverter for dependency injection
builder.Services.AddSingleton<IConverter<Customer, CustomerDto>, CustomerConverter>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Initialize the database.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetService<CustomerApiContext>();
    var dbInitializer = services.GetService<IDbInitializer>();
    dbInitializer.Initialize(dbContext);
}

//app.UseHttpsRedirection();
Task.Factory.StartNew(() =>
    new MessageListener(app.Services, ConnectionString).Start());


app.UseAuthorization();

app.MapControllers();

app.Run();