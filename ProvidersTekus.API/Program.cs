using Microsoft.EntityFrameworkCore;
using ProvidersTekus.DAL.DBContext;
using ProvidersTekus.IOC;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<BdprovidersContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL"));
});

//builder.Services.AddControllers();
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.InyectarDependencias(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("NuevaPolitica", app =>
    {
        app.AllowAnyOrigin();
        app.AllowAnyHeader();
        app.AllowAnyMethod();
        // app.AllowCredentials();
    });

});




var app = builder.Build();


app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
app.UseSwagger();
//app.UseSwaggerUI();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Providers");
    c.RoutePrefix = "swagger"; // http://providers.somee.com/swagger
});

app.UseCors("NuevaPolitica");

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger/index.html");
    return Task.CompletedTask;
});

app.MapControllers();

app.Run();
