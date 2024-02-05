using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ToEat.Domain.Data;
using ToEat.Application.Services;
using ToEat.Application.Strategies;
using ToEat.Application.Functions;
using ToEat.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ToEat.API.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ToEatContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ToEatContext") ?? throw new InvalidOperationException("Connection string 'ToEatContext' not found.")));

// Add services to the container.
builder.Services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<ToEatContext>()
        .AddDefaultTokenProviders();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IResponseHandlingStrategy, FunctionCallStrategy>();
builder.Services.AddScoped<IResponseHandlingStrategy, SimpleAnswerStrategy>();
builder.Services.AddScoped<ResponseHandlingService, ResponseHandlingService>();
builder.Services.AddScoped<GptIntegrationService>();
builder.Services.AddScoped<IFunction, AddInventoryItemFunction>();
builder.Services.AddScoped<FunctionRepository>();
builder.Services.AddScoped<ConversationService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})

// Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWTKey:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWTKey:ValidIssuer"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTKey:Secret"]))
                };
            });
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
