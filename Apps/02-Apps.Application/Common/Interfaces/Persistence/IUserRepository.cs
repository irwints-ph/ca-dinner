using Apps.Domain.Entities;

namespace Apps.Application.Common.Interfaces.Persistence;
public interface IUserRepository
{
    User? GetByEmail(string email);
    User? GetUser(string username);
    void Add(User user);
}
