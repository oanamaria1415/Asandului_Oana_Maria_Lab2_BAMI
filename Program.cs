using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Asandului_Oana_Maria_Lab2.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Asandului_Oana_Maria_Lab2Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Asandului_Oana_Maria_Lab2Context") ?? throw new InvalidOperationException("Connection string 'Asandului_Oana_Maria_Lab2Context' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    DbInitializer.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
