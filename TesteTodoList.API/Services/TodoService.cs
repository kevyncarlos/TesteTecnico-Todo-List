using Microsoft.EntityFrameworkCore;
using TesteTodoList.API.Data.Repositories.Interfaces;
using TesteTodoList.API.Models;
using TesteTodoList.API.Models.Entities;
using TesteTodoList.API.Models.Enums;
using TesteTodoList.API.Services.Interfaces;

namespace TesteTodoList.API.Services;

public class TodoService(ILogger<TodoService> logger, ITodoRepository todoRepository) : ITodoService
{
    private readonly ILogger<TodoService> _logger = logger;
    private readonly ITodoRepository _todoRepository = todoRepository;

    public async Task<Response<Todo>> AddAsync(Todo todo)
    {
        _logger.LogInformation($"{nameof(TodoService)} -> {nameof(AddAsync)} - {nameof(todo)}: {todo.Title}");

        try
        {
            todo.Status = TodoStatusEnum.Pending;
            todo.FinishDate = null;

            var result = await _todoRepository.AddOrUpdateAsync(todo);
            await _todoRepository.SaveChangesAsync();

            return Response<Todo>.GenerateWithSuccess("Todo add is successful", result);
        }
        catch (Exception ex)
        {
            return Response<Todo>.GenerateWithError("Todo add has an error: " + ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        _logger.LogInformation($"{nameof(TodoService)} -> {nameof(DeleteAsync)} - {nameof(id)}: {id}");

        try
        {
            var result = await _todoRepository.DeleteOrRestoreAsync(id);
            await _todoRepository.SaveChangesAsync();

            if (result == false)
                throw new Exception($"Todo with id: {id} not found.");

            return Response<bool>.GenerateWithSuccess("Todo delete is successful", result);
        }
        catch (Exception ex)
        {
            return Response<bool>.GenerateWithError("Todo add has an error: " + ex.Message);
        }
    }

    public async Task<Response<IEnumerable<Todo>>> GetAllAsync(int pageIndex = 0, int pageSize = 10, TodoStatusEnum? status = null)
    {
        _logger.LogInformation($"{nameof(TodoService)} -> {nameof(GetAllAsync)}");

        try
        {
            var query = await _todoRepository.GetPaginateByAsync(x => status == null || x.Status == status, pageIndex, pageSize);
            var result = await query.ToListAsync();

            if (result.Count == 0)
                throw new Exception($"Todos not found.");

            return Response<IEnumerable<Todo>>.GenerateWithSuccess("Get all Todos is successful", result);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<Todo>>.GenerateWithError("Get all Todos has an error: " + ex.Message);
        }
    }

    public async Task<Response<Todo>> GetByIdAsync(int id)
    {
        _logger.LogInformation($"{nameof(TodoService)} -> {nameof(GetByIdAsync)} - {nameof(id)}: {id}");

        try
        {
            var result = await _todoRepository.GetByIdAsync(id);

            if (result is null)
                throw new Exception($"Todo with id: {id} not found.");

            return Response<Todo>.GenerateWithSuccess("Get todo by id is successful", result);
        }
        catch (Exception ex)
        {
            return Response<Todo>.GenerateWithError("Get Todo by id has an error: " + ex.Message);
        }
    }

    public async Task<Response<Todo>> UpdateAsync(Todo todo)
    {
        _logger.LogInformation($"{nameof(TodoService)} -> {nameof(UpdateAsync)} - {nameof(todo)}: {todo.Title}");

        try
        {
            var result = await  _todoRepository.AddOrUpdateAsync(todo);
            await _todoRepository.SaveChangesAsync();

            return Response<Todo>.GenerateWithSuccess("Todo update is successful", result);
        }
        catch (Exception ex)
        {
            return Response<Todo>.GenerateWithError("Todo update has an error: " + ex.Message);
        }
    }

    public async Task<Response<Todo>> UpdateStatusAsync(int id)
    {
        try
        {
            var todo = await _todoRepository.GetByIdAsync(id);

            if(todo == null)
                return Response<Todo>.GenerateWithError("Todo not found: " + id);

            todo.FinishDate = todo.Status == TodoStatusEnum.Pending ? DateTime.Now : null;
            todo.Status = todo.Status == TodoStatusEnum.Pending ? TodoStatusEnum.Done : TodoStatusEnum.Pending;

            return await UpdateAsync(todo);
        }
        catch (Exception ex)
        {
            return Response<Todo>.GenerateWithError("Todo update has an error: " + ex.Message);
        }
    }
}
