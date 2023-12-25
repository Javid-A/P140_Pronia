using Microsoft.EntityFrameworkCore;
using P140_Pronia.DAL;
using P140_Pronia.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ProniaDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<LayoutService>();

var app = builder.Build();

app.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");


app.UseStaticFiles();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Run();
