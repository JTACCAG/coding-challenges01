using Api.Application.Authorization.Handlers;
using Api.Application.Authorization.Requirements;
using Api.Application.Enums;
using Api.Application.Middlewares;
using Api.Application.Repositories;
using Api.Application.Services;
using Api.Infrastructure.Mongo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters {
        ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
        ValidAudience = builder.Configuration["JwtConfig:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"]!)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
});


//DB
builder.Services.Configure<MongoSettings>(
    builder.Configuration.GetSection("Mongo")
);
builder.Services.AddSingleton<MongoService>();
builder.Services.AddHttpContextAccessor();

//Guard
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ActiveUser", policy => {
            policy.Requirements.Add(new ActiveUserRequirement());
        }
    ).AddPolicy("AdminActive", policy =>
    {
        policy.Requirements.Add(new ActiveUserRequirement());
        policy.Requirements.Add(new RoleRequirement(RoleEnum.Admin));
    });

builder.Services.AddScoped<IAuthorizationHandler, ActiveUserHandler>();
builder.Services.AddScoped<IAuthorizationHandler, RoleHandler>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<ReportRepository>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ReportService>();

QuestPDF.Settings.License = LicenseType.Community;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
