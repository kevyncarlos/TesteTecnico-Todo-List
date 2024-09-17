using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data;

public class TodoDbContext(DbContextOptions<TodoDbContext> options) : DbContext(options)
{
    public DbSet<Todo> Todos { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
                typeof(EntityBase).IsAssignableFrom(i.GenericTypeArguments[0])));
    }
}
