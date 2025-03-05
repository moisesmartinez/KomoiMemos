using Microsoft.EntityFrameworkCore;
using FinanceMemos.API.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FinanceMemos.API.Services;
using FluentValidation.AspNetCore;
using FluentValidation.AspNetCore;
using FluentValidation;
using FinanceMemos.API.Repositories.Interfaces;
using FinanceMemos.API.Repositories;
using FinanceMemos.API.Services.Interfaces;
using Microsoft.AspNetCore.Diagnostics;
using FinanceMemos.API.CustomExceptions;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//MM 01: Add DbContext
builder.Services.AddDbContext<KomoiMemosDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


//MM 02: Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// MM 03: Add JwtTokenService
builder.Services.AddScoped<JwtTokenService>();

// MM 04: Add FluentValidation
builder.Services.AddFluentValidationAutoValidation()
               .AddFluentValidationClientsideAdapters();

// MM 04: Register validators from the assembly containing the specified type
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

//MM 05: Register Auth repo and service
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<JwtTokenService>();

//MM 05: Adding Mediatr
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//MM 07: Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:52240")
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});


var app = builder.Build();

//MM 07: Use CORS middleware
app.UseCors("AllowReactApp");

//MM 06: Add exception handling middleware
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        if (exceptionHandlerPathFeature?.Error is InputValidationException inputValidationException)
        {
            var response = new
            {
                Message = inputValidationException.Message
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        else if (exceptionHandlerPathFeature?.Error is ValidationException validationException)
        {
            var errors = validationException.Errors
                .Select(e => new { e.PropertyName, e.ErrorMessage })
                .ToList();
            var response = new { Message = "Validation failed.", Errors = errors };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
        else
        {
            var response = new
            {
                Message = "An unexpected error occurred."
            };
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    });
});

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
