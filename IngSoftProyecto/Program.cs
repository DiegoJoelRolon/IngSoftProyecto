using IngSoftProyecto.Context;
using IngSoftProyecto.CQRS.Commands;
using IngSoftProyecto.CQRS.Queries;
using IngSoftProyecto.Mapper;
using IngSoftProyecto.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>options.UseMySql(connectionString,new MySqlServerVersion(new Version(8, 0, 43))));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ActividadQuery>();
builder.Services.AddScoped<ActividadCommand>();
builder.Services.AddScoped<ActividadService>();
builder.Services.AddScoped<ActividadMapper>();

builder.Services.AddScoped<MiembroQuery>();
builder.Services.AddScoped<MiembroCommand>();
builder.Services.AddScoped<MiembroService>();
builder.Services.AddScoped<MiembroMapper>();
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
