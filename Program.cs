using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TodoApi.Services;
using TodoApi.Services.Interface;

var builder = WebApplication.CreateBuilder(args);


// Configuration
builder.Configuration.AddJsonFile("appsettings.json");
var configuration = builder.Configuration;

// Database context
builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 8;
})
    .AddEntityFrameworkStores<TodoContext>()
    .AddDefaultTokenProviders();

// Authentication
builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["Authentication:Audience"],
        ValidIssuer = configuration["Authentication:Issuer"],
        RequireExpirationTime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Authentication:ApiKey"])),
        ValidateIssuerSigningKey = true
    });


builder.Services.AddControllers();

builder.Services.AddScoped<IUserService, UserService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
