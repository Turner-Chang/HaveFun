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

// �]�wCookie + JWT����
var jwt = builder.Configuration.GetSection("Jwt:secret").Get<string>();
builder.Services.AddAuthentication(options =>
{
    // �]�w�w�]���{�Ҥ�׬� Cookie �{�Ҥ��
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // �]�w�w�]���D�Ԥ�׬� Cookie �{�Ҥ��
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    //�]�w�p�G���ҥ��ѡA�L�n��V�����}
    options.LoginPath = "/Login";
    // �]�w Cookie ���L���ɶ��� 1 ��
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    // �]�w Cookie ���̤j���Įɶ��� 1 ��
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
    // �]�w Cookie ���W�٬� "secret"
    options.Cookie.Name = "cookieSecret";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    // �ҥηưʹL���ɶ��A�p�G�ϥΪ̦b���Ĵ����X�ݡA�L���ɶ��|�۰ʩ���
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

// ���U�۩w�q�����n��
app.UseMiddleware<BackendAuthMiddleware>();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//�K�[chathub
app.MapHub<ChatHub>("/chathub");




app.Run();
