using Final_UI.Helpers.SignalR;
using Final_UI.Profiles;
using Final_UI.Services.Implementations;
using Microsoft.AspNetCore.SignalR;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddHttpsRedirection(options => {
//  options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
//  options.HttpsPort = 44360;
//});

// Example of bypassing SSL certificate validation (use with caution)
ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;


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

// app.UseExceptionHandler("/Error/InternalServerError");

// app.UseMiddleware<ExceptionHandlingMiddleware>();

// app.UseStatusCodePagesWithReExecute("/Home/Error", "?statusCode={0}");

// app.UseHttpsRedirection();

// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapHub<NotificationHub>("/notificationHub");

app.Run();