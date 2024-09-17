using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data.Repositories.Interfaces;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> GetByLoginAndPasswordAsync(string login, string password);
}
