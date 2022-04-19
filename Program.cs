using UserService.DAL;
using UserService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    //o.Authority = Configuration["Jwt:Authority"];
    //o.Audience = Configuration["Jwt:Audience"];
    o.Authority = "https://keycloak.sebananasprod.nl/auth/realms/Kwetter";
    o.Audience = "account";
    o.Events = new JwtBearerEvents()
    {
        OnAuthenticationFailed = c =>
        {
            c.NoResult();

            c.Response.StatusCode = 500;
            c.Response.ContentType = "text/plain";
            if (builder.Environment.IsDevelopment())
            {
                return c.Response.WriteAsync(c.Exception.ToString());
            }
            return c.Response.WriteAsync("An error occured processing your authentication.");
        }
    };
});


// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Inject repo
builder.Services.AddScoped<IUserRepository, UserRepository>();


if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySQL("server=localhost;port=3306;database=kwetter;user=root;password=CEzmBKLB8?f5s!G7"));
}
else
{
    builder.Services.AddDbContext<UserContext>(options =>
    options.UseMySQL("server=38.242.248.109;port=3306;database=kwetter;user=root;password=CEzmBKLB8?f5s!G7"));
}

builder.Services.AddControllers();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    //app.UseMigrationsEndPoint();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers().RequireCors(MyAllowSpecificOrigins);


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UserContext>();
    context.Database.EnsureCreated();
    UserInitializer.Initialize(context);
}


app.Run();