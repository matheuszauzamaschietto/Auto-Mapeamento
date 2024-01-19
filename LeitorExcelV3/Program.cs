
using LeitorExcelV3.Services;
using OfficeOpenXml;

namespace LeitorExcelV3;

public class Program
{
    public static void Main(string[] args)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddHttpClient("client");
        builder.Services.AddScoped<WorksheetService>();
        builder.Services.AddScoped<DirectoryService>();

        var app = builder.Build();

        app.UseCors(cr =>
        {
            cr.AllowAnyHeader();
            cr.AllowAnyMethod();
            cr.AllowAnyOrigin();
        });

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
    }
}
