using AutoMapper;
using Final_UI.Helpers.Filters;
using Final_UI.Helpers.Middlewares;
using Final_UI.Profiles;
using Final_UI.Services.Implementations;
using Final_UI.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpsRedirection(options => {
  options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
  options.HttpsPort = 44360;
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

// Custom services
builder.Services.AddScoped<AuthFilter>();
builder.Services.AddScoped<AdminOrSuperAdminFilter>();
builder.Services.AddScoped<SuperAdminFilter>();
builder.Services.AddScoped<ICrudService, CrudService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDataService, DataService>();

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

app.UseHttpsRedirection();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
  name: "default",
  pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();