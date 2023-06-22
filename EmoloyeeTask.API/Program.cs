using EmoloyeeTask.API.Auth;
using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using EmoloyeeTask.Data.Repositories;
using EmployeeTask.Data.Repositories;
using Industry.BS.Data.Interfaces;
using Industry.BS.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = AuthOptions.Issuer,
            ValidAudience = AuthOptions.Audience,
            RequireSignedTokens = true,
            IssuerSigningKey = AuthOptions.PublicKey
        };
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["token"];
                if (!string.IsNullOrWhiteSpace(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
    .RequireAuthenticatedUser()
    .Build();
});
builder.Services.AddSwaggerGen(c =>
{
    var filePath = Path.Combine(AppContext.BaseDirectory, "Industry.BS.Shared.xml");
    var sharedFilePath = Path.Combine(AppContext.BaseDirectory, "Industry.BS.API.xml");

    c.IncludeXmlComments(filePath);
    c.IncludeXmlComments(sharedFilePath);
    c.SwaggerDoc("v1", new OpenApiInfo()
    {
        TermsOfService = new Uri("http://industryerp.site"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("http://industryerp.site/contact")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("http://industryerp.site/license")
        }
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly("EmoloyeeTask.Data.Migrations"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", builder =>
    {
        builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
}
);

builder.Services.AddScoped<IDeviceCodeRepository, DeviceCodeRepository>();
builder.Services.AddScoped<IAllowedCashedDeviceCodeRepository, CachedDeviceCodeRepository>();

builder.Services.AddScoped<IDbRepository<Employee>, EmployeeRepository>();
builder.Services.AddScoped<IDbRepository<Division>, DivisionRepository>();
builder.Services.AddScoped<IDbRepository<Project>, ProjectRepository>();
builder.Services.AddScoped<IDbRepository<Issue>, IssueRepository>();
builder.Services.AddScoped<IDbRepository<LaborCost>, LaborCostRepository>();
builder.Services.AddScoped<IDbRepository<Document>, DocumentRepository>();
builder.Services.AddScoped<IEmployeeStatsRepository, EmployeeStatsRepository>();

builder.Services.AddTransient<IMailSender, EmailSender>();

builder.Services.AddMemoryCache();

builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();


app.UseForwardedHeaders();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("MyPolicy");

//app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<DeviceCodeCheckMiddleware>();
app.UseMiddleware<JwtAuthMiddleware>();

app.Run();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
