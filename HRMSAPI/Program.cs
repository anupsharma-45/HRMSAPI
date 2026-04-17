using HRMSAPI.Data;
using HRMSAPI.Interfaces;
using HRMSAPI.Repositories;
using HRMSAPI.Services;
using HRMSAPI.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using FluentValidation;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

#region SERILOG CONFIGURATION

Log.Logger = new LoggerConfiguration()
	.ReadFrom.Configuration(builder.Configuration)
	.Enrich.FromLogContext()
	.WriteTo.Console()
	.WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
	.CreateLogger();

builder.Host.UseSerilog();

#endregion

#region ADD SERVICES

builder.Services.AddControllers();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAutoMapper(typeof(Program));

#endregion

#region CORS CONFIGURATION

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAngular",
		policy =>
		{
			policy.WithOrigins("http://localhost:4200")
				  .AllowAnyHeader()
				  .AllowAnyMethod();
		});
});

#endregion

#region DATABASE CONFIGURATION

builder.Services.AddDbContext<AppDbContext>(options =>
	options.UseSqlServer(
		builder.Configuration.GetConnectionString("DefaultConnection")));

#endregion

#region JWT AUTHENTICATION

var jwtKey = builder.Configuration["Jwt:Key"]
			 ?? throw new InvalidOperationException("JWT Key Missing");

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme =
		JwtBearerDefaults.AuthenticationScheme;

	options.DefaultChallengeScheme =
		JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
	options.RequireHttpsMetadata = false;

	options.SaveToken = true;

	options.TokenValidationParameters =
		new TokenValidationParameters
		{
			ValidateIssuerSigningKey = true,

			IssuerSigningKey =
				new SymmetricSecurityKey(
					Encoding.UTF8.GetBytes(jwtKey)
				),

			ValidateIssuer = true,

			ValidIssuer =
				builder.Configuration["Jwt:Issuer"],

			ValidateAudience = true,

			ValidAudience =
				builder.Configuration["Jwt:Audience"],

			ValidateLifetime = true,

			ClockSkew = TimeSpan.Zero
		};
});

#endregion

#region AUTHORIZATION POLICIES

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("RequireSuperAdmin",
		policy => policy.RequireRole("SuperAdmin"));

	options.AddPolicy("RequireOrgAdmin",
		policy => policy.RequireRole("OrgAdmin"));

	options.AddPolicy("RequireEmployee",
		policy => policy.RequireRole("Employee"));
});

#endregion

#region DEPENDENCY INJECTION

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IOrganizationService, OrganizationService>();

#endregion

#region SWAGGER CONFIGURATION

builder.Services.AddSwaggerGen(options =>
{
	options.SwaggerDoc("v1",
		new OpenApiInfo
		{
			Title = "HRMS API",
			Version = "v1",
			Description = "Enterprise HRMS Backend API"
		});

	options.AddSecurityDefinition("Bearer",
		new OpenApiSecurityScheme
		{
			Name = "Authorization",

			Type = SecuritySchemeType.Http,

			Scheme = "bearer",

			BearerFormat = "JWT",

			In = ParameterLocation.Header,

			Description =
				"Enter JWT Token Like: Bearer {your token}"
		});

	options.AddSecurityRequirement(
		new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference =
						new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
				},
				Array.Empty<string>()
			}
		});
});

#endregion

var app = builder.Build();

#region MIDDLEWARE PIPELINE

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();

	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint(
			"/swagger/v1/swagger.json",
			"HRMS API V1");

		options.RoutePrefix = "swagger";
	});
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

app.UseAuthentication();

app.UseMiddleware<AuditLoggingMiddleware>();

app.UseAuthorization();

app.MapControllers();

#endregion

#region RUN APP

try
{
	Log.Information("HRMS API Starting...");

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Application crashed unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}

#endregion