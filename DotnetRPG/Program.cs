using DotnetRpg.Services.CharactersService;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using DotnetRpg.Middleware;
using DotnetRpg.Core;
using DotnetRpg.Services.Settings;
using DotnetRpg.Services.AuthService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(new JsonFormatter(), "Logs/important-logs.json", restrictedToMinimumLevel: LogEventLevel.Warning)
    .WriteTo.File("Logs/DailyLogs/all-daily.logs", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Debug()
    .CreateLogger();

var appName = builder.Configuration.GetSection("AppName").Value;
Log.Debug("App Name: {appName}", appName);
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));


builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((c) =>
{
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddSingleton<ISettingsSingletonService, SettingsSingletonService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ICharactersService, CharactersService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ExceptionHandlerMiddleware>();
//-- JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer((options) =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JwtSettings:SecretKey").Value))
        // ValidAudience = builder.Configuration.GetSection("JwtSettings:Audience").Value,
        // ValidIssuer = builder.Configuration.GetSection("JwtSettings:Issuer").Value    
    };
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Gdev")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMyExceptionHandler();
// app.UseExceptionHandler((exceptionHandlerOptions) =>
//     {
//         exceptionHandlerOptions.Run(async context =>
//         {
//             var contextFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
//             if (contextFeature != null)
//             {
//                  Log.Error(contextFeature.Error.GetBaseException(),"Error");
//                 // object MyLogEvents = null;
//                 //-- Write Error logger               
//                 // app.Logger.LogError(MyLogEvents.UnHandleError, contextFeature.Error.Message);

//                 //-- Error 500 response
//                 context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//                 context.Response.ContentType = System.Net.Mime.MediaTypeNames.Application.Json;

//                 var err = new
//                 {
//                     Code = context.Response.StatusCode,
//                     contextFeature.Error.Message
//                 };

//                 await context.Response.WriteAsJsonAsync(err);
//             }
//         });
//     });

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
