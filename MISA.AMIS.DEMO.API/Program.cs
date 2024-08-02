using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MISA.AMIS.DEMO.API;
using MISA.AMIS.DEMO.Core;
using MISA.AMIS.DEMO.Core.Resources;
using MISA.AMIS.DEMO.Infrastructure;
using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { 
        new CultureInfo("vi-VN"),
        new CultureInfo("en-US") 
    };
    options.DefaultRequestCulture = new RequestCulture("vi-VN");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Reset to PascalCase
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            context.HttpContext.Response.ContentType = "application/json";
            var modelState = context.ModelState;
            var errors = new Dictionary<string, List<string>>();
            foreach (var entry in modelState)
            {
                var fieldErrors = entry.Value.Errors.Select(e => e.ErrorMessage);
                errors[entry.Key] = fieldErrors.ToList();
            }
            var result = new MISAException
            {
                DevMessage = MISAResources.InValidMsg_InValidInput,
                UserMessage = MISAResources.InValidMsg_InValidInformation,
                TraceId = "",
                MoreInfo = "",
                Errors = errors,
            };
            return new BadRequestObjectResult(result);
        };
    })
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) => factory.Create(typeof(MISAResources));
    });


// CORS Config
builder.Services.AddCors(policyBuilder =>
    policyBuilder.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
            .AllowAnyMethod()
            .WithOrigins("http://localhost:8080", "http://localhost:8081", "http://localhost:8082", "http://localhost:8083")
            .AllowCredentials();
    })
);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
//builder.Services.AddSingleton<IConfiguration>(Configuration);

// Dependency Injection
builder.Services.AddTransient<IMISADbContext, MariaDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IValidateEmployee, ValidateEmployee>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerGroupRepository, CustomerGroupRepository>();
builder.Services.AddScoped<ICustomerGroupService, CustomerGroupService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IBaseRepository<Employee>, BaseRepository<Employee>>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJWTProvider, JWTProvider>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IMappingFileService, MappingFileService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IMemoryCache, MemoryCache>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddCookie()

// Adding Jwt Bearer
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
        ClockSkew = TimeSpan.Zero
    };
    options.Events = new JwtBearerEvents
    {
        OnChallenge = async context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            var exception = new MISAException()
            {
                UserMessage = MISAResources.InValidMsg_AuthenticationFail,
                DevMessage = MISAResources.InValidMsg_UnauthorizedUser,
                TraceId = "",
                MoreInfo = "",
            };
            await context.Response.WriteAsJsonAsync(exception);
        },
    };
});

//Bulid App
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();

app.UseRequestLocalization();

app.UseMiddleware<AuthenticationMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();

app.Run();
