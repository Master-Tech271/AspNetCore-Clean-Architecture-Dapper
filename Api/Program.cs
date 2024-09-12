using Api;
using Api.Application;
using Api.Application.Common;
using Api.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

Log.Logger = new LoggerConfiguration()
    .CreateLogger();


try
{
    Log.Information("starting server.");

    var builder = WebApplication.CreateBuilder(args);
    {
        //serilog
        builder.Host.UseSerilog((context, loggerConfiguration) =>
        {
            loggerConfiguration.ReadFrom.Configuration(context.Configuration);
        });

        // Add CORS services
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin() // Allows all origins
                                  .AllowAnyMethod() // Allows any HTTP methods (GET, POST, etc.)
                                  .AllowAnyHeader() // Allows any headers
                                  );
        });
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                builder => builder.WithOrigins("https://your-frontend-app.com") // Specify your frontend origin
                                  .AllowAnyMethod() // Allow any HTTP methods (GET, POST, etc.)
                                  .AllowAnyHeader() // Allow any headers
                                  .AllowCredentials()); // Allow credentials (for cookies, authorization headers, etc.)
        });

        // Add Authentication service.
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
                ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                ValidAudience = builder.Configuration["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
            };
        });

        // Add authorization policies
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireUserRole", policy => policy.RequireRole("User"));
            options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("SystemAdmin"));
        });

        builder.Services.AddTransient<ILogService, LogService>();

        builder.Services.AddPresentation()
                     .AddApplication()
                     .AddInfrastructure(builder.Configuration);

    }

    var app = builder.Build();
    {
        app.UseExceptionHandler("/error");
        app.UseSerilogRequestLogging();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // Use CORS middleware
        app.UseCors("AllowAllOrigins"); // Apply the CORS policy
        //use when need specific origin cors and for prod environment
        //app.UseCors("AllowSpecificOrigin");

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();


        app.Map("/error", (HttpContext context, ILogService _logger) =>
        {
            Exception? exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

            _logger.LogError("Global Error", exception);
            return Results.Problem();
        });

        app.Run();

    }

}
catch (Exception ex)
{
    Log.Fatal(ex, "server terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}