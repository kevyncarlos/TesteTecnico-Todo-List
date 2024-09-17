using TesteTodoList.API.Data.Repositories.Interfaces;
using TesteTodoList.API.Models;
using TesteTodoList.API.Models.Entities;
using TesteTodoList.API.Services.Interfaces;

namespace TesteTodoList.API.Services;

public class UserService(ILogger<TodoService> logger, IUserRepository userRepository) : IUserService
{
    private readonly ILogger<TodoService> _logger = logger;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Response<User>> LoginAsync(User user)
    {
        _logger.LogInformation($"{nameof(UserService)} -> {nameof(LoginAsync)} - {nameof(user)}: {user.Login}");

        try
        {
            var result = await _userRepository.GetByLoginAndPasswordAsync(user.Login, user.Password);

            if(result == null)
                return Response<User>.GenerateWithError("Login not found");
            else
                return Response<User>.GenerateWithSuccess("Login successful", result);
        }
        catch (Exception ex)
        {
            return Response<User>.GenerateWithError("Todo add has an error: " + ex.Message);
        }

    }
}
