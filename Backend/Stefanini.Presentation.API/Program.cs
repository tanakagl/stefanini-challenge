using Stefanini.Application.UseCases;
using Stefanini.Domain.Interfaces;
using Stefanini.Infrastructure.Persistence;
using Stefanini.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registro automático de assemblys com scrutor
builder.Services.Scan(scan => scan
    .FromAssembliesOf(typeof(CreateUserUseCase), typeof(UserRepository))
    
    // Registra os repositórios
    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    
    // Registra os Use Cases
    .AddClasses(classes => classes.InNamespaces("Stefanini.Application.UseCases"))
        .AsSelf()
        .WithScopedLifetime()
);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

