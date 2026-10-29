using AspNetCore.Unobtrusive.Ajax;
using InstructorWebClient.Domain;
using InstructorWebClient.Infrastructure.IRepository;
using InstructorWebClient.Infrastructure.IService;
using InstructorWebClient.Repository;
using InstructorWebClient.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InstructorWebClient.DataAccess.Models.InstructorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InstructorDb")));

// 🔑 Add Authentication with Cookie scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index"; // Your login path
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // In production
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.ExpireTimeSpan = TimeSpan.FromHours(1); // Session duration
        options.SlidingExpiration = true; // Renew cookie on activity
    });

builder.Services.AddControllersWithViews();
builder.Services.AddUnobtrusiveAjax();

// 🔑 Add services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IAddressService, AddressService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<IEmailSenderService, EmailSenderService>();
builder.Services.AddTransient<IFileReaderService, FileReaderService>();
builder.Services.AddTransient<ISessionManagerService, SessionManagerService>();
builder.Services.AddTransient<ISessionService, SessionService>();
builder.Services.AddTransient<IStudentCourseService, StudentCourseService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<IStudentSessionService, StudentSessionService>();
builder.Services.AddTransient<ISummaryService, SummaryService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IUserTypeService, UserTypeService>();

//🔑 Add repository
builder.Services.AddTransient<IAddressRepository, AddressRepository>();
builder.Services.AddTransient<ICourseRepository, CourseRepository>();
builder.Services.AddTransient<IModuleHourLocationRepository, ModuleHourLocationRepository>();
builder.Services.AddTransient<ISessionModuleRepository, SessionModuleRepository>();
builder.Services.AddTransient<ISessionRepository, SessionRepository>();
builder.Services.AddTransient<IStudentCourseRepository, StudentCourseRepository>();
builder.Services.AddTransient<IStudentRepository, StudentRepository>();
builder.Services.AddTransient<IStudentSessionRepository, StudentSessionRepository>();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IUserTypeRepository, UserTypeRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // ✅ Should come before routing

app.UseRouting();

app.UseSession(); // ✅ Session before auth

app.UseAuthentication(); // ✅ Must come before Authorization
app.UseAuthorization();

app.UseUnobtrusiveAjax();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();