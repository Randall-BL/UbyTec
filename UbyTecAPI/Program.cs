using Microsoft.EntityFrameworkCore;
using UbyTecAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar los servicios para el contenedor de la aplicación.
builder.Services.AddControllers();
builder.Services.AddDbContext<UbyTecContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("UbyTecDatabase")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder => builder
            .AllowAnyOrigin()  // Permite cualquier origen
            .AllowAnyMethod()  // Permite cualquier método (GET, POST, PUT, DELETE)
            .AllowAnyHeader()); // Permite cualquier encabezado
});

var app = builder.Build();

// Configurar la tubería de solicitud HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

// Aplicar CORS
app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
