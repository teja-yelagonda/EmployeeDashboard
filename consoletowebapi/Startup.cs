using consoletowebapi.DBContext;
using consoletowebapi.Helper;
using consoletowebapi.Repository;
using consoletowebapi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;
using System.Text.Json.Serialization;
using Hangfire;
using consoletowebapi.DatabaseLayer.IRepository;
using consoletowebapi.DatabaseLayer.Repository;
using consoletowebapi.BusinessLayer.IServices;
using consoletowebapi.BusinessLayer.Services;
using consoletowebapi.Models;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)  // Change static to instance method
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp",
                builder =>
                {
                    builder.WithOrigins("http://localhost:4200")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
        });
        var Key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]);

        services.AddDbContext<OrganizationContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = false;
              options.SaveToken = true;
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  ValidateIssuerSigningKey = true,
                  ValidIssuer = _configuration["JwtSettings:Issuer"],
                  ValidAudience = _configuration["JwtSettings:Audience"],
                  ClockSkew = TimeSpan.Zero,
                  IssuerSigningKey = new SymmetricSecurityKey(Key)
              };
          });


        services.AddAuthorization();

        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1", Description = "Simple Test API" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT authentication Header",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference=new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });

        services.Configure<SwaggerSettings>(_configuration.GetSection("SwaggerSettings"));


        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
        services.AddScoped<ILeaveRequestService, LeaveRequestService>();
        services.AddScoped<ISwaggerEndpointsRepository, SwaggerEndpointsRepository>();
        services.AddScoped<IUserService, UserService>();
        //services.AddScoped<SwaggerEndpointServices>();

        services.AddControllers(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                            .RequireAuthenticatedUser()
                            .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }).AddJsonOptions(options=>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(_configuration.GetConnectionString("DefaultConnection"));
            });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseMiddleware<GlobalException>();
        app.UseCors("AllowAngularApp");
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "myswagger";
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "MY API v1");
            c.RoutePrefix = string.Empty;
        });


        app.UseRouting();
        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<SwaggerEndpointServices>();

        app.UseHangfireDashboard();
        app.UseHangfireServer();
        RecurringJob.AddOrUpdate<LeaveRequestService>("LeaveProcessing", x => x.LeaveProcessing(), "35 22 * * 5", TimeZoneInfo.Local);

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
