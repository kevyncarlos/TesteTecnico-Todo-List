using Microsoft.EntityFrameworkCore;
using TesteTodoList.API.Data.Repositories.Interfaces;
using TesteTodoList.API.Models.Entities;

namespace TesteTodoList.API.Data.Repositories;

public class UserRepository(TodoDbContext context) : RepositoryBase<User>(context), IUserRepository
{
    public async Task<User?> GetByLoginAndPasswordAsync(string login, string password)
    {
        var user = await (GetBy(x => x.Login.Equals(login) && x.Password.Equals(password))).FirstOrDefaultAsync();

        return user;
    }
}
