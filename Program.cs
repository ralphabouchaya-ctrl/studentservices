using Microsoft.EntityFrameworkCore;
using MyRazorApp.Data;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages()
       .AddRazorRuntimeCompilation();  // enables runtime compilation

// If you also have controllers
builder.Services.AddControllersWithViews()
       .AddMvcOptions(options =>
       {
           options.SuppressAsyncSuffixInActionNames = true;
       })
       .AddRazorRuntimeCompilation();
// Razor pages




// Local fallback for development (optional)





var connectionString =
    $"Server={host};Port={port};Database={name};User={user};Password={pass};";

// Register EF Core MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

// Add session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapRazorPages();
app.MapGet("/", context =>
{
    context.Response.Redirect("/SignIn");
    return Task.CompletedTask;
});

app.Run();
