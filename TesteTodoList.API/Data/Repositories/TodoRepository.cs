using Microsoft.EntityFrameworkCore;
using TesteTodoList.API.Data.Repositories.Interfaces;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data.Repositories;

public class TodoRepository(TodoDbContext context) : RepositoryBase<Todo>(context), ITodoRepository
{
}
