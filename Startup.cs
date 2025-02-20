using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using otel_advisor_webApp.Data;
using otel_advisor_webApp.Interfaces;
using otel_advisor_webApp.Services;
using System.Net.Mail;
using System.Net;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyMethod()
                                  .AllowAnyHeader());
        });

        services.AddScoped<ReservationConfirmedService>();

        services.AddControllers();

        services.AddDbContext<HotelContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

        // Swagger konfigürasyonu
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
        });

        //Add cors policy for allowing all origins
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()       
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
        });

        var emailConfig = Configuration.GetSection("EmailSettings");
        services.AddSingleton(new SmtpClient(emailConfig["SmtpServer"])
        {
            Port = int.Parse(emailConfig["SmtpPort"]),
            Credentials = new NetworkCredential(emailConfig["SmtpUser"], emailConfig["SmtpPass"]),
            EnableSsl = true
        });

        services.AddTransient<IEmailService, EmailService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseCors("AllowAllOrigins");

        app.UseRouting();

        // Use CORS policy
        app.UseCors("AllowAllOrigins");

        app.UseAuthorization();

        // Swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            c.RoutePrefix = string.Empty; 
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
