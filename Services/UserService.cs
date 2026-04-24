using LibrarySystem.Data;
using LibrarySystem.Models;
using LibrarySystem.Responses;

namespace LibrarySystem.Services;

public class UserService
{
    private readonly MysqlDbContext _context;
    
    public UserService(MysqlDbContext context)
    {
        _context = context;
    }

    public ServiceResponse<IEnumerable<User>> GetAllUsers()
    {
        var users = _context.users.ToList();
        return new ServiceResponse<IEnumerable<User>>()
        {
            Success = true,
            Data = users
        };
    }

    public ServiceResponse<User> SaveUser(User user)
    {
        var UserExists = _context.users.FirstOrDefault(u => u.Email == user.Email);

        if (UserExists != null)
        {
            return new ServiceResponse<User>()
            {
                Success = false,
                Data = user,
                Message = "User already exists"
            };
        }
        
        _context.users.Add(user);
        _context.SaveChanges();
        return new ServiceResponse<User>()
        {
            Success = true,
            Data = user,
            Message = "User created successfully"
        };
    }

    public ServiceResponse<User> GetUserById(int id)
    {
        var user = _context.users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            return new ServiceResponse<User>()
            {
                Success = true,
                Data = user,
                Message = "User found"
            };
        }

        return new ServiceResponse<User>()
        {
            Success = false,
            Data = user,
            Message = "User not found"
        };
    }

    public ServiceResponse<User> UpdateUser(User user)
    {
        var userDb = _context.users.Find(user.Id);

        userDb.Name = user.Name;
        userDb.Email = user.Email;
        userDb.Phone = user.Phone;
        _context.SaveChanges();

        return new ServiceResponse<User>()
        {
            Success = true,
            Data = user,
            Message = "User updated successfully"
        };
    }
    
    public ServiceResponse<User> DeleteUser(User user)
    {
        var userDb = _context.users.Find(user.Id);

        _context.users.Remove(userDb);
        _context.SaveChanges();

        return new ServiceResponse<User>()
        {
            Success = true,
            Data = user,
            Message = "User removed successfully"
        };
    }
    
}