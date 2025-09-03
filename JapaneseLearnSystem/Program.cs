using JapaneseLearnSystem.Models;
using JapaneseLearnSystem.Services;
using Microsoft.EntityFrameworkCore;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<MemberIdGenerator>();
// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<dbJapaneseLearnSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbJapaneseLearnSystemConnection")));









//////////////////////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "ºÆ¨g¥ñ¯S/{controller=Home}/{action=Index}/{id?}");

app.Run();
