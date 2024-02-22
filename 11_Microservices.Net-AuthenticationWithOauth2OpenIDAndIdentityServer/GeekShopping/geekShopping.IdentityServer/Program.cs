using geekShopping.IdentityServer.Configuration;
using geekShopping.IdentityServer.Initializer;
using geekShopping.IdentityServer.Model;
using geekShopping.IdentityServer.Model.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DB Connection
var connection = builder.Configuration["MySQLConnection:MySQLConnectionString"];
builder.Services.AddDbContext<MySQLContext>(
    options => options.UseMySql(connection,
                     new MySqlServerVersion(new Version(8, 2, 0)
    )));

//Services Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<MySQLContext>()
    .AddDefaultTokenProviders();

var geekIdentiyBuilder = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
}).AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
      .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
      .AddInMemoryClients(IdentityConfiguration.Clients)
      .AddAspNetIdentity<ApplicationUser>();

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

geekIdentiyBuilder.AddDeveloperSigningCredential();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

var initializer = app.Services.CreateScope().ServiceProvider.GetService<IDbInitializer>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();

initializer.Initialize();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
