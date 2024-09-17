using Microsoft.AspNetCore.Mvc;
using TesteTodoList.API.Models;
using TesteTodoList.API.Models.Entities;
using TesteTodoList.API.Models.Enums;
using TesteTodoList.API.Services.Interfaces;

namespace TesteTodoList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class TodosController(ILogger<TodosController> logger, ITodoService todoService) : ControllerBase
{
    private readonly ILogger<TodosController> _logger = logger;
    private readonly ITodoService _todoService = todoService;

    [HttpGet]
    public async Task<Response<IEnumerable<Todo>>> GetAsync(int pageIndex = 0, int pageSize = 10, TodoStatusEnum? status = null)
    {
        _logger.LogInformation($"{nameof(TodosController)} -> {nameof(GetAsync)}");

        return await _todoService.GetAllAsync(pageIndex, pageSize, status);
    }

    [HttpGet("{id}")]
    public async Task<Response<Todo>> GetByIdAsync(int id)
    {
        _logger.LogInformation($"{nameof(TodosController)} -> {nameof(GetByIdAsync)}");

        return await _todoService.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<Response<Todo>> PostAsync(Todo todo)
    {
        _logger.LogInformation($"{nameof(TodosController)} -> {nameof(PostAsync)} - {nameof(todo)}: {todo.Title}");

        return await _todoService.AddAsync(todo);
    }

    [HttpPut]
    public async Task<Response<Todo>> PutAsync(Todo todo)
    {
        _logger.LogInformation($"{nameof(TodosController)} -> {nameof(PutAsync)} - {nameof(todo)}: {todo.Title}");

        return await _todoService.UpdateAsync(todo);
    }

    [HttpPatch("UpdateStatus/{id}")]
    public async Task<Response<Todo>> UpdateStatus(int id)
    {
        _logger.LogInformation($"{nameof(TodosController)} -> {nameof(UpdateStatus)} - {nameof(id)}: {id}");

        return await _todoService.UpdateStatusAsync(id);
    }

    [HttpDelete("{id}")]
    public async Task<Response<bool>> DeleteAsync(int id)
    {
        _logger.LogInformation($"{nameof(TodosController)} -> {nameof(DeleteAsync)} - {nameof(id)}: {id}");

        return await _todoService.DeleteAsync(id);
    }
}
