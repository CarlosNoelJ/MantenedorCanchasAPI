using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ReservaCanchasAPI.Data;
using ReservaCanchasAPI.Models;
using ReservaCanchasAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios
builder.Services.AddDbContext<ReservaCanchasContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar los repositorios
builder.Services.AddScoped<IRepository<TipoCancha>, Repository<TipoCancha>>();
builder.Services.AddScoped<IRepository<Canchas>, Repository<Canchas>>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Reserva Canchas API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Reserva Canchas API V1");
    });
    app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
}
else
{
    app.UseHttpsRedirection();
    app.UseCors(x => x.WithOrigins("http://example.com").AllowAnyMethod().AllowAnyHeader());
}

app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
