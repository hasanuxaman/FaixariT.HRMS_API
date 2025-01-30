using FaixariT.HRMS.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FaixariT.HRMS.Service;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext and configure the connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("HRMSConnection")));

// Register JwtService and other services
builder.Services.AddSingleton<JwtService>();

builder.Services.AddControllers();

// Add Swagger/OpenAPI support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS to allow all origins, methods, and headers
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()  // Allow all origins
               .AllowAnyMethod()  // Allow all HTTP methods (GET, POST, etc.)
               .AllowAnyHeader()); // Allow all headers
});

// Configure JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use the "AllowAll" CORS policy
app.UseCors("AllowAll");

app.UseAuthentication();  // Ensure authentication is enabled
app.UseAuthorization();   // Ensure authorization is enabled

app.MapControllers();  // Map controllers for routing

app.Run();
