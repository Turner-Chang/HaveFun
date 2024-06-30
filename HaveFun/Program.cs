using HaveFun;
using HaveFun.Common;
using HaveFun.Models;
//using HaveFun.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Logging.AddAzureWebAppDiagnostics();
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "HaveFun.session";
    options.IdleTimeout = TimeSpan.FromDays(1);
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});


var connHaveFunStr = builder.Configuration.GetConnectionString("HaveFunDbContext");
builder.Services.AddSqlServer<HaveFunDbContext>(connHaveFunStr);
builder.Services.AddSignalR();
builder.Services.AddScoped<SaveImage>();
builder.Services.AddSingleton<PasswordSecure>();
builder.Services.AddScoped<SendEmail>();
builder.Services.AddSingleton<Jwt>();
builder.Services.AddSingleton<DESSecure>();
builder.Services.AddSingleton<GoogleOAuth>();
//builder.Services.AddTransient<ExceptionHandleMiddleware>();
builder.Services.AddScoped<MembershipService>();
//builder.Services.AddScoped<PostServices>();

// 設定Cookie + JWT驗證
var jwt = builder.Configuration.GetSection("Jwt:secret").Get<string>();
builder.Services.AddAuthentication(options =>
{
    // 設定預設的認證方案為 Cookie 認證方案
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // 設定預設的挑戰方案為 Cookie 認證方案
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    //設定如果驗證失敗，他要轉向的網址
    options.LoginPath = "/Login";
    // 設定 Cookie 的過期時間為 1 天
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    // 設定 Cookie 的最大有效時間為 1 天
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
    // 設定 Cookie 的名稱為 "secret"
    options.Cookie.Name = "cookieSecret";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // 啟用滑動過期時間，如果使用者在有效期內訪問，過期時間會自動延長
    options.SlidingExpiration = true;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt)
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["JwtSecret"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
   
}
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandleMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();

app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

// 註冊自定義中介軟體
app.UseMiddleware<BackendAuthMiddleware>();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//添加chathub
app.MapHub<ChatHub>("/chathub");




app.Run();
