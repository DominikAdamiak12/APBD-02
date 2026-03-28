namespace ConsoleApp1.Services;

using ConsoleApp1.Models;

public class UserService
{
    private readonly List<User> _users = new();

    public void Add(User user)
    {
        _users.Add(user);
    }

    public User? GetById(int id)
    {
        return _users.FirstOrDefault(u => u.Id == id);
    }

    public List<User> GetAll()
    {
        return _users.ToList();
    }
}