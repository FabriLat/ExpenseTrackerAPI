using System.Text;
using Application.interfaces;
using Application.services;
using Domain.Interfaces;
using Infrastructure.data;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using static Infrastructure.Services.AuthenticationService;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SplitEasy API",
        Version = "v1",
        Description = "API SplitEasy"
    });

    // Configuración para autenticación JWT
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", // Esquema en minúsculas
        Description = "Pega el token JWT generado en el formato: Bearer {token}",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization"
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
            new List<string>()
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Si los DTOs están en otro proyecto (por ejemplo, Application)
    var appXmlFile = "Application.xml"; // Ajusta según el nombre del proyecto
    var appXmlPath = Path.Combine(AppContext.BaseDirectory, appXmlFile);
    if (File.Exists(appXmlPath))
    {
        c.IncludeXmlComments(appXmlPath);
    }
});


builder.Services.AddDbContext<GestionGastosContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DBConnectionString"),
        new MySqlServerVersion(new Version(8, 0, 21))));


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["AuthenticationService:Issuer"],
            ValidAudience = builder.Configuration["AuthenticationService:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["AuthenticationService:SecretForKey"]))
        };
    });


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AnonymousOnly", policy =>
        policy.RequireAssertion(context => !context.User.Identity.IsAuthenticated));
});


//SERVICIOS
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<ITeamService, TeamService>();


//REPOSITORIOS
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.Configure<AuthenticationServiceOptions>(
    builder.Configuration.GetSection("AuthenticationService"));
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SplitEasy API v1"));
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();