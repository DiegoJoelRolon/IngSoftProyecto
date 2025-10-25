using IngSoftProyecto.Context;
using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 43))));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ActividadQuery>();
builder.Services.AddScoped<ActividadCommand>();
builder.Services.AddScoped<ActividadService>();
builder.Services.AddScoped<ActividadMapper>();

builder.Services.AddScoped<AsistenciaQuery>();
builder.Services.AddScoped<AsistenciaCommand>();
builder.Services.AddScoped<AsistenciaMaper>();
builder.Services.AddScoped<AsistenciaService>();

builder.Services.AddScoped<ClaseService>();
builder.Services.AddScoped<ClaseQuery>();
builder.Services.AddScoped<ClaseCommand>();
builder.Services.AddScoped<ClaseMapper>();

builder.Services.AddScoped<EntrenadorService>();
builder.Services.AddScoped<EntrenadorQuery>();
builder.Services.AddScoped<EntrenadorCommand>();
builder.Services.AddScoped<EntrenadorMapper>();

builder.Services.AddScoped<MembresiasService>();
builder.Services.AddScoped<MembresiaQuery>();
builder.Services.AddScoped<MembresiaCommand>();
builder.Services.AddScoped<MembresiaMapper>();

builder.Services.AddScoped<MembresiaXMiembroQuery>();
builder.Services.AddScoped<MembresiaXMiembroCommand>();
builder.Services.AddScoped<MembresiaXMiembroMapper>();
builder.Services.AddScoped<MembresiaXMiembroService>();

builder.Services.AddScoped<MiembroQuery>();
builder.Services.AddScoped<MiembroCommand>();
builder.Services.AddScoped<MiembroService>();
builder.Services.AddScoped<MiembroMapper>();

builder.Services.AddScoped<MiembroXClaseCommand>();
builder.Services.AddScoped<MiembroXClaseQuery>();
builder.Services.AddScoped<MiembroXClaseMapper>();
builder.Services.AddScoped<MiembroXClaseService>();

builder.Services.AddScoped<PagoService>();
builder.Services.AddScoped<PagoQuery>();
builder.Services.AddScoped<PagoCommand>();
builder.Services.AddScoped<PagoMapper>();


builder.Services.AddScoped<GenericMapper>();

builder.Services.AddScoped<TipoDeAsistenciaQuery>();
builder.Services.AddScoped<TipoDeAsistenciaCommand>();
builder.Services.AddScoped<TipoDeAsistenciaService>();

builder.Services.AddScoped<TipoDeMembresiaQuery>();
builder.Services.AddScoped<TipoDeMembresiaCommand>();
builder.Services.AddScoped<TipoDeMembresiaService>();

builder.Services.AddScoped<TipoDeMiembroQuery>();
builder.Services.AddScoped<TipoDeMiembroCommand>();
builder.Services.AddScoped<TipoDeMiembroMapper>();




builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});


var app = builder.Build();

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
