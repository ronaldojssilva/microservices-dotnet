using Microsoft.EntityFrameworkCore;
using GeekShopping.Email.Model.Context;
using GeekShopping.Email.Repository;
using GeekShopping.Email.MessageConsumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DB Connection
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(
    options => options.UseMySql(connection,
                     new MySqlServerVersion(new Version(8, 2, 0)
    )));

var contextBuilder = new DbContextOptionsBuilder<MySQLContext>();
contextBuilder.UseMySql(connection, new MySqlServerVersion(new Version(8, 2, 0)));

builder.Services.AddSingleton(new EmailRepository(contextBuilder.Options));
builder.Services.AddScoped<IEmailRepository, EmailRepository>();
builder.Services.AddHostedService<RabbitMQPaymentConsumer>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
