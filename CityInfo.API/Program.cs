var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddMvc(); MVC hizmetleri
// builder.Services.AddControllersWithViews();// Controllerları viewler ile birlikte kulllanma
builder.Services.AddControllers(); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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