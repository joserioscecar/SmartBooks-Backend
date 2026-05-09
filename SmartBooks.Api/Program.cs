using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using QuestPDF.Infrastructure;
using SmartBooks.Api.Middleware;
using SmartBooks.Application;
using SmartBooks.Application.Interfaces;
using SmartBooks.Application.Validators.Clientes;
using SmartBooks.Infrastructure;
using SmartBooks.Infrastructure.Http;
using SmartBooks.Infrastructure.Options;
using SmartBooks.Infrastructure.Persistence;
using System.Text;


QuestPDF.Settings.License = LicenseType.Community;



var builder = WebApplication.CreateBuilder(args);




builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.Configure<EmailOptions>(
    builder.Configuration.GetSection(EmailOptions.SectionName));

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection(JwtOptions.SectionName));

var jwtOptions = builder.Configuration
    .GetSection(JwtOptions.SectionName)
    .Get<JwtOptions>()!;


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IClientContext, ClientContext>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidAudience = jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.Key)
            ),
            ClockSkew = TimeSpan.Zero
        };
    });


builder.Services.AddAuthorization();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowAll", b =>
    {
        b.AllowAnyOrigin()
         .AllowAnyMethod()
         .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Bearer {tu_token}"
    });

    opt.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Id = "Bearer",
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddProblemDetails();
//builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();


builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddValidatorsFromAssembly(typeof(CreateClienteValidator).Assembly);


var app = builder.Build();
//app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartBook API v1");
    c.RoutePrefix = string.Empty;
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


/*
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SmartBookDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    var retries = 2;
    while (retries-- > 0)
    {
        try
        {
            logger.LogInformation("Aplicando migraciones...");
            await db.Database.MigrateAsync();
            logger.LogInformation(" Migraciones aplicadas correctamente.");
            break;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error completo al migrar. Reintentos restantes: {retries}", retries);
            if (retries == 0) throw; 
            await Task.Delay(3000);
        }
    }

}

*/


app.Run();
