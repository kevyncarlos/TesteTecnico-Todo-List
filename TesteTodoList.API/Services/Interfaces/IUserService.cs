using TesteTodoList.API.Models;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Services.Interfaces;

public interface IUserService
{
    Task<Response<User>> LoginAsync(User user);
}
