using JapaneseLearnSystem.Models;
using JapaneseLearnSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;





var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";        // 未登入被導向的頁面
        options.LogoutPath = "/Account/Logout";      // 登出路徑（POST）
        options.AccessDeniedPath = "/Account/Denied";// 權限不足導向
        options.SlidingExpiration = true;            // 自動延長 Cookie 有效期
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });


builder.Services.AddScoped<MemberIdGenerator>();
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<dbJapaneseLearnSystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbJapaneseLearnSystemConnection")));


builder.Services.AddDbContext<dbJapaneseLearnSystemContextG2>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("dbJapaneseLearnSystemConnection")));

builder.Services.AddScoped<QuestionGenerate>();








//////////////////////////////////////////////////////////////////////////////////////////////////

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// 加這行！！
app.UseAuthentication();


app.UseAuthorization();

// 1. 先註冊特殊路由
//多層次路由


app.MapAreaControllerRoute(
    name : "AdminArea",
    areaName : "Admin",
    pattern: "Admin/{controller=MemberManagements}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "MembersArea",
    areaName: "Members",
    pattern: "Members/{controller=MemberCenterController}/{action=Index}/{id?}");



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");





app.Run();
