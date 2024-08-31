using Final_UI.Helpers.SignalR;
using Final_UI.Profiles;
using Final_UI.Services.Implementations;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using Final_UI.Helpers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(opt => {
    opt.ListenAnyIP(8081, listenOptions => {
        listenOptions.UseHttps("/https/ayazumman.pfx", "ayazumman");
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

// Custom services
builder.Services.AddScoped<AuthFilter>();
builder.Services.AddScoped<AdminOrSuperAdminFilter>();
builder.Services.AddScoped<SuperAdminFilter>();
builder.Services.AddScoped<ICrudService, CrudService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IExcelReportService, ExcelReportService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg => {
  cfg.AddProfile(new MapProfile());
}).CreateMapper());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error/InternalServerError");
  app.UseHsts();
}
else {
  app.UseDeveloperExceptionPage();
}

app.UseExceptionHandler("/Error/InternalServerError");

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
