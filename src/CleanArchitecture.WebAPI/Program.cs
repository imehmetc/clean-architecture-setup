using CleanArchitecture.Infrastructure;
using CleanArchitecture.Application;
using Scalar.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.OData;
using CleanArchitecture.WebAPI.Controllers;
using CleanArchitecture.WebAPI.Modules;

namespace CleanArchitecture.WebAPI;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        
        builder.AddServiceDefaults();
        builder.Services.AddApplication();
        builder.Services.AddInfrastrusture(builder.Configuration);

        builder.Services.AddCors(); // openapi kullanılabilmesi için mutlaka cors politikası yazılmalı.
        builder.Services.AddOpenApi(); // openapi
        builder.Services.AddControllers().AddOData(opt => opt // openapi ekstra sorgu
                    .Select()
                    .Filter()
                    .Count()
                    .Expand()
                    .OrderBy()
                    .SetMaxTop(null)
                    .AddRouteComponents("odata", AppODataController.GetEdmModel())
                    );

        builder.Services.AddRateLimiter(x =>
            x.AddFixedWindowLimiter("fixed", cfg =>
            {
                cfg.QueueLimit = 100;
                cfg.Window = TimeSpan.FromSeconds(1);
                cfg.PermitLimit = 100;
                cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            }));

        builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

        var app = builder.Build();

		app.MapOpenApi(); // openapi
		app.MapScalarApiReference(); // scalar
		
        
        app.MapDefaultEndpoints();

        app.UseCors(x => x // Openapi için cors
            .AllowAnyHeader()
            .AllowCredentials()
            .AllowAnyMethod()
            .SetIsOriginAllowed(t => true)
            );

        app.RegisterRoutes();

        app.UseExceptionHandler();

        app.MapControllers().RequireRateLimiting("fixed");

		app.Run();
    }
}
