using FeedbackApi.Services; // Asegúrate de incluir el namespace donde está MongoDbContext
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios

// Lee la configuración de MongoDB desde appsettings.json
var mongoSettings = builder.Configuration.GetSection("MongoDB");
var mongoConnectionString = mongoSettings.GetValue<string>("ConnectionString");
var mongoDatabaseName = mongoSettings.GetValue<string>("DatabaseName");

// Configura MongoDB como un servicio singleton
builder.Services.AddSingleton<IMongoClient>(sp =>
    new MongoClient(mongoConnectionString));

// Registra MongoDbContext como un servicio singleton
builder.Services.AddSingleton<MongoDbContext>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return new MongoDbContext(builder.Configuration);
});

// Agrega servicios de controladores
builder.Services.AddControllers();

// Configura CORS para permitir conexiones de cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configura Swagger para documentación de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline de la aplicación

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Habilitar HTTPS Redirection
app.UseHttpsRedirection();

// Habilitar CORS
app.UseCors("AllowAll");

// Mapear controladores
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Ejecutar la aplicación
app.Run();
