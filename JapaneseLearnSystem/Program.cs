using JapaneseLearnSystem.Models;
using JapaneseLearnSystem.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;





var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";        // ���n�J�Q�ɦV������
        options.LogoutPath = "/Account/Logout";      // �n�X���|�]POST�^
        options.AccessDeniedPath = "/Account/Denied";// �v�������ɦV
        options.SlidingExpiration = true;            // �۰ʩ��� Cookie ���Ĵ�
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

// �[�o��I�I
app.UseAuthentication();


app.UseAuthorization();

// 1. �����U�S�����
//�h�h������


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
