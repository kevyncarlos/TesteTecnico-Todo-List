using Microsoft.EntityFrameworkCore;
using TesteTodoList.API.Data;
using TesteTodoList.API.Data.Repositories;
using TesteTodoList.API.Data.Repositories.Interfaces;
using TesteTodoList.API.Services;
using TesteTodoList.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyOrigin()   // Permite todas as origens
            .AllowAnyMethod()   // Permite todos os métodos HTTP
            .AllowAnyHeader()); // Permite todos os cabeçalhos
});

builder.Services.AddControllers();
builder.Services.AddDbContext<TodoDbContext>(
    options => options.UseSqlServer("name=ConnectionStrings:TodoDb"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Creation of database and insertion of administrator user, for testing purposes only.
    using IServiceScope scope = app.Services.CreateScope();
    TodoDbContext db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.EnsureDeleted();
    db.Database.EnsureCreated();
    db.Users.Add(new TesteTodoList.API.Models.Entities.User()
    {
        CreatedDate = DateTime.Now,
        IsActive = true,
        Login = "kevyn",
        Password = "123"
    });
    db.SaveChanges();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
