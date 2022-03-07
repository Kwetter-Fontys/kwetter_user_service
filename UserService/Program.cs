using UserService.DAL;
using UserService.DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Controllers;

UserController userController;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Inject repo
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddDbContext<UserContext>(options =>
  options.UseMySQL("server=localhost;database=kwetter;user=root;password="));
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
app.MapControllers();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UserContext>();
    context.Database.EnsureCreated();
    UserInitializer.Initialize(context);
}


app.Run();