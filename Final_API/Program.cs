using AutoMapper;
using Final_API.Filters;
using Final_Business.Exceptions;
using Final_Business.Profiles;
using Final_Business.Services.Implementations;
using Final_Business.Services.Interfaces;
using Final_Business.Validators;
using Final_Core.Entities;
using Final_Data;
using Final_Data.Repositories.Implementations;
using Final_Data.Repositories.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Hangfire.MemoryStorage;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options => {
  options.InvalidModelStateResponseFactory = context => {
    var errors = context.ModelState.Where(x => x.Value!.Errors.Count > 0)
    .Select(x => new RestExceptionError(x.Key, x.Value!.Errors.First().ErrorMessage)).ToList();
    return new BadRequestObjectResult(new { message = "", errors });
  };
});

builder.Services.AddDbContext<AppDbContext>(option => {
  //option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
  //option.UseSqlServer(builder.Configuration.GetConnectionString("DGKConnection"));
  option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt => {
  opt.Password.RequireNonAlphanumeric = false;
  opt.Password.RequiredLength = 8;
  opt.Password.RequireUppercase = false;
  opt.Password.RequireLowercase = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton(provider => new MapperConfiguration(cfg => {
  cfg.AddProfile(new MapProfile(provider.GetService<IHttpContextAccessor>()!));
}).CreateMapper());

//Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
  c.SwaggerDoc("user", new OpenApiInfo {
    Title = "User API",
    Version = "v1"
  });

  c.SwaggerDoc("admin", new OpenApiInfo {
    Title = "Admin API",
    Version = "v1"
  });

  c.SwaggerDoc("auth", new OpenApiInfo {
    Title = "Auth API",
    Version = "v1"
  });

  c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
    In = ParameterLocation.Header,
    Description = "Please insert JWT with Bearer into field",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey
  });

  c.AddSecurityRequirement(new OpenApiSecurityRequirement
  {
    {
      new OpenApiSecurityScheme
      {
        Reference = new OpenApiReference
        {
          Type = ReferenceType.SecurityScheme,
          Id = "Bearer"
        }
      },
      Array.Empty<string>()
    }
  });

  // Apply the document filters
  c.DocumentFilter<AdminDocumentFilter>();
  c.DocumentFilter<UserDocumentFilter>();
  c.DocumentFilter<AuthDocumentFilter>();
});



//Fluent Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<SliderCreateValidator>();

//Custom Services
builder.Services.AddScoped<IUserAuthService, UserAuthService>();

builder.Services.AddScoped<IUserHouseService, UserHouseService>();
builder.Services.AddScoped<IAdminHouseService, AdminHouseService>();
builder.Services.AddScoped<ISliderService, SliderService>();
builder.Services.AddScoped<IFeatureService, FeatureService>();
builder.Services.AddScoped<IHouseImageService, HouseImageService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddScoped<IHouseRepository, HouseRepository>();
builder.Services.AddScoped<ISliderRepository, SliderRepository>();
builder.Services.AddScoped<IFeatureRepository, FeatureRepository>();
builder.Services.AddScoped<IHouseImageRepository, HouseImageRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => {
  loggerConfiguration
  .ReadFrom.Configuration(hostingContext.Configuration);
});

//Micro-elements
builder.Services.AddFluentValidationRulesToSwagger();


// Configure JWT Authentication
builder.Services.AddAuthentication(opt => {
  opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
  opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
  opt.TokenValidationParameters = new TokenValidationParameters {
    ValidAudience = builder.Configuration.GetSection("JWT:Audience").Value,
    ValidIssuer = builder.Configuration.GetSection("JWT:Issuer").Value,
    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:Secret").Value!))
  };
});

// Configure Hangfire
builder.Services.AddHangfire(config =>
  config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseDefaultTypeSerializer()
    .UseMemoryStorage()); // Use in-memory storage

// Add Hangfire server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
  app.UseSwagger();
  app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/user/swagger.json", "User API V1");
    c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API V1");
    c.SwaggerEndpoint("/swagger/auth/swagger.json", "Auth API V1");

    // Customize the Swagger UI
    c.UseRequestInterceptor("(request) => { request.headers.Authorization = 'Bearer ' + request.headers.Authorization; return request; }");
  });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

// Hangfire Dashboard
app.UseHangfireDashboard();

// Schedule the daily email job
//RecurringJob.AddOrUpdate<EmailService>("send-daily-flower-email",
//  service => service.SendFlowerEmail(),
//  "0 4 * * *"); // Cron expression for 08:00 AM daily

app.UseStaticFiles();

app.MapControllers();

app.UseMiddleware<Final_API.Middlewares.ExceptionHandlerMiddleware>();

app.Run();

