using Microsoft.AspNetCore.Mvc;
using TesteTodoList.API.Models;
using TesteTodoList.API.Models.Entities;
using TesteTodoList.API.Models.Enums;
using TesteTodoList.API.Services.Interfaces;

namespace TesteTodoList.API.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController(ILogger<UsersController> logger, IUserService userService) : ControllerBase
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IUserService _userService = userService;

    [HttpPost("Login")]
    public async Task<Response<User>> Login(User user)
    {
        _logger.LogInformation($"{nameof(UsersController)} -> {nameof(Login)} -> {nameof(user)}: {user.Login}");

        return await _userService.LoginAsync(user);
    }
}
