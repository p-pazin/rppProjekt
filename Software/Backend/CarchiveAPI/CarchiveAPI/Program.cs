using System.Text.Json.Serialization;
using System.Text;
using CarchiveAPI.Data;
using CarchiveAPI.Repositories;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<VehicleRepository>();
builder.Services.AddScoped<VehicleServices>();
builder.Services.AddScoped<ContactServices>();
builder.Services.AddScoped<OfferServices>();
builder.Services.AddScoped<OfferRepository>();
builder.Services.AddScoped<OfferVehicleRepository>();
builder.Services.AddScoped<CompanyServices>();
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<CompanyRepository>();
builder.Services.AddScoped<StatsServices>();
builder.Services.AddScoped<StatsRepository>();
builder.Services.AddScoped<AdServices>();
builder.Services.AddScoped<AdRepository>();
builder.Services.AddScoped<ContractRepository>();
builder.Services.AddScoped<ContractServices>();
builder.Services.AddScoped<InvoiceServices>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<ReservationServices>();
builder.Services.AddScoped<ReservationRepository>();
builder.Services.AddScoped<InsuranceServices>();
builder.Services.AddScoped<InsuranceRepository>();
builder.Services.AddScoped<PenaltyServices>();
builder.Services.AddScoped<PenaltyRepository>();
builder.Services.AddScoped<LocationServices>();
builder.Services.AddScoped<LocationRepository>();
builder.Services.AddTransient<EmailService>();



builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("3nVhe7YYN5KAl06W5qLEeQRMATqfqpZ5K305e8")),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ValidateLifetime = true 
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Your API",
        Version = "v1"
    });

    // Add JWT Authentication Support
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token. Example: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddMvcCore().AddXmlSerializerFormatters();

builder.Services.AddMvc(options =>
{
    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
});

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages")),
    RequestPath = "/UploadedImages"
});

app.UseAuthentication(); 
app.UseAuthorization(); 

app.MapControllers();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Urls.Add($"http://0.0.0.0:{port}");
app.Urls.Add($"https://0.0.0.0:7209");

app.Run();
