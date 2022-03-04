using UserService.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Controllers;

UserController userController;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserContext>(options =>
  options.UseMySQL("server=localhost;database=kwetter;user=root;password="));

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<UserContext>();
    context.Database.EnsureCreated();
    UserInitializer.Initialize(context);
}
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();
app.MapGet("/users", () =>
{
    //This should be done by dependency injection however IDK
    userController = new UserController(app.Services.CreateScope().ServiceProvider.GetRequiredService<UserContext>());
    return userController.GetAllUsers();
})
.WithName("GetUsers");

app.Run();