
using Apps.Application.Common.Interfaces.Persistence;
using Apps.Domain.Entities;

namespace Apps.Infrastructure.Persistence;
public class UserRepository: IUserRepository
{
    //Temporary storage: In-Memory
    // Make static so that it is shared across all instances of UserRepository, not new instance per request
    private static readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetUser(string username)
    {
        return _users.SingleOrDefault(u => u.Username == username);
    }

    public User? GetByEmail(string email)
    {
        return _users.SingleOrDefault(u => u.Email == email);    
    }

}
