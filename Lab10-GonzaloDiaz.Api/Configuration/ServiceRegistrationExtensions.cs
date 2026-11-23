using System.Security.Claims;
using System.Text;
using Lab10_GonzaloDiaz.Application.UserCase.AuthUseCase;
using Lab10_GonzaloDiaz.Application.UserCase.ResponseUseCases;
using Lab10_GonzaloDiaz.Application.UserCase.RoleUseCases;
using Lab10_GonzaloDiaz.Application.UserCase.TicketUseCase;
using Lab10_GonzaloDiaz.Application.UserCase.UserRoleUseCase;
using Lab10_GonzaloDiaz.Application.UserCase.UserUseCase;
using Lab10_GonzaloDiaz.Infraestructure.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

namespace Lab10_GonzaloDiaz.Api.Configuration;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureServices(configuration);

        services.AddScoped<RegisterUseCase>();
        services.AddScoped<LoginUseCase>();

        services.AddScoped<GetUserByIdUseCase>();

        services.AddScoped<CreateRoleUseCase>();
        services.AddScoped<GetRolesUseCase>();
        services.AddScoped<AssignRoleUseCase>();

        services.AddScoped<CreateTicketUseCase>();
        services.AddScoped<GetTicketsUseCase>();
        services.AddScoped<GetTicketByIdUseCase>();

        services.AddScoped<CreateResponseUseCase>();
        services.AddScoped<GetResponseByIdUseCase>();
        services.AddScoped<GetResponsesByTicketUseCase>();

        services.AddControllers();
        services.AddEndpointsApiExplorer();

        var secretKey = configuration["Jwt:SecretKey"];

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new InvalidOperationException(
                "Falta configurar Jwt:SecretKey.");
        }

        var keyBytes = Encoding.UTF8.GetBytes(secretKey);

        if (keyBytes.Length < 32)
        {
            throw new InvalidOperationException(
                "Jwt:SecretKey debe tener al menos 32 bytes.");
        }

        var issuer = configuration["Jwt:Issuer"];

        if (string.IsNullOrWhiteSpace(issuer))
        {
            throw new InvalidOperationException(
                "Falta configurar Jwt:Issuer.");
        }

        var audience = configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(audience))
        {
            throw new InvalidOperationException(
                "Falta configurar Jwt:Audience.");
        }

        services.AddAuthentication(
            JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.MapInboundClaims = false;

                options.TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = issuer,
                        ValidAudience = audience,

                        IssuerSigningKey =
                            new SymmetricSecurityKey(keyBytes),

                        NameClaimType = ClaimTypes.Name,
                        RoleClaimType = ClaimTypes.Role,

                        ValidAlgorithms = new[]
                        {
                            SecurityAlgorithms.HmacSha256
                        },

                        ClockSkew = TimeSpan.Zero
                    };
            });

        services.AddAuthorization();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API de Tickets",
                Version = "v1",
                Description = "Laboratorio 10 de Gonzalo Diaz"
            });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Ingresa solamente el token JWT."
                });

            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecuritySchemeReference(
                            "Bearer",
                            document),
                        new List<string>()
                    }
                });
        });

        return services;
    }
}