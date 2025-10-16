using Backend.Application.UseCases;
using Backend.Domain.Interfaces;
using Backend.Infrastructure.Persistence;
using Backend.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Registro automático de assemblys com scrutor
builder.Services.Scan(scan => scan
    .FromAssemblyOf<CreateUserUseCase>()
    
    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    
    .AddClasses(classes => classes.Where(type => type.Name.EndsWith("UseCase")))
        .AsSelf()
        .WithScopedLifetime()
);

builder.Services.AddControllers();
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
