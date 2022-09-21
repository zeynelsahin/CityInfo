using CityInfo.API;
using CityInfo.API.DbContexts;
using CityInfo.API.Services;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Serilog;

Log.Logger = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console().WriteTo.File("logs/CitiyInfo-.txt", rollingInterval: RollingInterval.Day).CreateLogger();
var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();  
// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();
// Add services to the container.

builder.Services.AddSingleton<CitiesDataStore>();

builder.Services.AddDbContext<CityInfoContext>(optionsBuilder => optionsBuilder.UseSqlite(builder.Configuration["ConnectionStrings:CityInfoConnectionString"]));

// builder.Services.AddMvc(); MVC hizmetleri
// builder.Services.AddControllersWithViews();// Controllerları viewler ile birlikte kulllanma
builder.Services.AddControllers(options =>
    {
        options.ReturnHttpNotAcceptable = true;
    }).AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();//Desteklenmyen formatlar için default format da verinin gönderilmemesi için
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<FileExtensionContentTypeProvider>();
builder.Services.AddTransient<LocalMailService>(); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
// app.UseRouting();
app.UseAuthorization(); 
app.MapControllers();

// app.Run(async (context)=>
// {
//     await context.Response.WriteAsync("Zeynel");
// });

app.Run();