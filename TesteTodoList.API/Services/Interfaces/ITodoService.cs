using TesteTodoList.API.Models;
using TesteTodoList.API.Models.Entities;
using TesteTodoList.API.Models.Enums;

namespace TesteTodoList.API.Services.Interfaces;

public interface ITodoService
{
    Task<Response<Todo>> AddAsync(Todo todo);
    Task<Response<bool>> DeleteAsync(int id);
    Task<Response<IEnumerable<Todo>>> GetAllAsync(int pageIndex, int pageSize, TodoStatusEnum? status);
    Task<Response<Todo>> GetByIdAsync(int id);
    Task<Response<Todo>> UpdateAsync(Todo todo);
    Task<Response<Todo>> UpdateStatusAsync(int id);
}
