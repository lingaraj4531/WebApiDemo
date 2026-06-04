using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebApiDemo.Data;
using WebApiDemo.Services;

namespace WebApiDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Databases & Dependency Injection
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddDbContext<StudentDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("StudentConnection")));

            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IStudentService, StudentService>();

            // 2. Simple Versioning Engine Setup
            builder.Services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            // 3. 📉 SIMPLIFIED SWAGGER GEN (No more extra helper classes!)
            builder.Services.AddSwaggerGen(options =>
            {
                // Explicitly declare your two versions cleanly
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApiDemo Docs - V1", Version = "1.0" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "WebApiDemo Docs - V2", Version = "2.0" });

                // Keeps identical names from crashing by utilizing full namespaces
                options.CustomSchemaIds(type => type.FullName);
            });

            var app = builder.Build();

            // 4. HTTP Request Pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                // 📉 SIMPLIFIED DROPDOWN (Explicitly mapped to your static endpoints)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "V1 Docs");
                    options.SwaggerEndpoint("/swagger/v2/swagger.json", "V2 Docs");
                });
            }

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}