using System.Reflection;
using System.Text.Json.Serialization;
using FoodDelivery.Api.Data;
using FoodDelivery.Api.DTOs;
using FoodDelivery.Api.Interfaces;
using FoodDelivery.Api.Middleware;
using FoodDelivery.Api.Repositories;
using FoodDelivery.Api.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerExamplesFromAssemblyOf<OrderRequestExample>();

var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "FoodDelivery.Api",
        Version = "v1",
        Description = "Internal food delivery order management API."
    });

    options.IncludeXmlComments(xmlPath);
    options.EnableAnnotations();
    options.ExampleFilters();
});

builder.Services.AddDbContext<FoodDeliveryDbContext>(options =>
    options.UseInMemoryDatabase("FoodDeliveryDb"));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularLocalhost", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<FoodDeliveryDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "FoodDelivery.Api v1");
    options.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseCors("AngularLocalhost");
app.MapControllers();

app.Run();
