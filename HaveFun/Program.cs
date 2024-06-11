using HaveFun.Common;
using HaveFun.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var connHaveFunStr = builder.Configuration.GetConnectionString("HaveFunDbContext");
builder.Services.AddSqlServer<HaveFunDbContext>(connHaveFunStr);
builder.Services.AddSignalR();
builder.Services.AddScoped<SaveImage>();
builder.Services.AddSingleton<PasswordSecure>();
builder.Services.AddScoped<SendEmail>();
builder.Services.AddSingleton<Jwt>();

// �]�wCookie + JWT����
var jwtKey = builder.Configuration.GetSection("Jwt:secret").Get<string>();
builder.Services.AddAuthentication(options =>
{
    // �]�w�w�]���{�Ҥ�׬� Cookie �{�Ҥ��
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    // �]�w�w�]���D�Ԥ�׬� Cookie �{�Ҥ��
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options =>
{
    // �]�w Cookie ���L���ɶ��� 1 ��
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    // �]�w Cookie ���̤j���Įɶ��� 1 ��
    options.Cookie.MaxAge = TimeSpan.FromDays(1);
    // �]�w Cookie ���W�٬� "secret"
    options.Cookie.Name = "secret";
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
            Encoding.UTF8.GetBytes(jwtKey)
        ),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["secret"];
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
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
//�K�[chathub
app.MapHub<ChatHub>("/chathub");

app.Run();
