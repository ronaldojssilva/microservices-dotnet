using GeekShopping.Web.Services;
using GeekShopping.Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

//Add API Services
builder.Services.AddHttpClient<IProductService, ProductService>(c =>
  c.BaseAddress = new Uri(builder.Configuration["ServicesUrls:ProductAPI"])
);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Security Authentication 1
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", c => c.ExpireTimeSpan = TimeSpan.FromMinutes(10))
    .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = builder.Configuration["ServicesUrls:IdentityServer"];
            options.GetClaimsFromUserInfoEndpoint = true;
            options.ClientId = "geek_shopping";
            options.ClientSecret = "my_super_secret";
            options.ResponseType = "code";
            options.ClaimActions.MapJsonKey("role", "role", "role");
            options.ClaimActions.MapJsonKey("sub", "sub", "sub");
            options.TokenValidationParameters.NameClaimType = "name";
            options.TokenValidationParameters.RoleClaimType = "role";
            options.Scope.Add("geek_shopping");
            options.SaveTokens = true;
        }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

//Security Authentication 2
app.UseAuthentication(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
