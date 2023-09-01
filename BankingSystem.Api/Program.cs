using BankingSystem.Core.Interfaces;
using BankingSystem.Core.Services;
using BankingSystem.Models.Entities;
using BankingSystem.Repositories.Interfaces;
using BankingSystem.Repositories.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IGenericRepository<Account>, GenericRepository<Account>>();
builder.Services.AddSingleton<IGenericRepository<User>, GenericRepository<User>>();
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddSingleton<IAccountService, AccountService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Banking System API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) { }

//Just for testing purposes due to this assessment, I enabled swagger not only for the development environment
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking System API V1");
});

app.UseHttpsRedirection();

app.UseCors(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthorization();

app.MapControllers();

app.Run();
