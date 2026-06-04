using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiDemo.Data;
using WebApiDemo.Services;

namespace WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("StudentConnection")));

            builder.Services.AddScoped<IEmployeeService,EmployeeService>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            builder.Services.AddApiVersioning(options =>
            {
                // If a client doesn't specify a version, default to v1.0
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                // Advertise the supported versions in the response headers (Good practice!)
                options.ReportApiVersions = true;
                // Configure how .NET reads the version. 
                // This setting tells it to look at the URL path (e.g., /api/v1/student)
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })

            .AddApiExplorer(options =>
            {
                // This formats the version name in Swagger as 'v1', 'v2', etc.
                options.GroupNameFormat = "'v'VVV";
                // This tells Swagger to substitute the route parameter 'v{version}' 
                // automatically so you don't have to type it manually every time.
                options.SubstituteApiVersionInUrl = true;
            });


            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // 🌀 1. REGISTER SWAGGER GENERATION Safely
            builder.Services.AddSwaggerGen();

            // 🌀 2. THE DYNAMIC SECRET: Register the dynamic options class below!
            builder.Services.ConfigureOptions<ConfigureSwaggerGenOptions>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    // Pull the detected versions list out of the running engine
                    var provider = app.Services.GetRequiredService<Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider>();

                    // 🌀 3. DYNAMIC UI LOOP: Automatically generates a dropdown menu for every detected version
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        var url = $"/swagger/{description.GroupName}/swagger.json";
                        var name = description.GroupName.ToUpperInvariant() + " Docs";
                        c.SwaggerEndpoint(url, name);
                    }
                });
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }

    // 🌀 4. THE DYNAMIC ENGINE CLASS (Paste this right here outside the Main method!)
    public class ConfigureSwaggerGenOptions : Microsoft.Extensions.Options.IConfigureOptions<Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions>
    {
        private readonly Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerGenOptions(Asp.Versioning.ApiExplorer.IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        public void Configure(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            // Automatically scans controllers and builds a separate schema group for each version found!
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = $"WebApiDemo Docs - {description.GroupName.ToUpperInvariant()}",
                    Version = description.ApiVersion.ToString()
                });
            }
        }
    }
}
