using System.Data;
using Domain.DTO;
using Domain.Entities;
using Domain.Events;
using Infrastructure.DTO;
using Infrastructure.Helpers;
using MediatR;

namespace Infrastructure.SqlCommands;

public class UserCommands : SqlCommandHelper
{
    private readonly SqlConnectionFactory _connectionFactory;
    public UserCommands(SqlConnectionFactory connectionFactory, IMediator mediator) : base(mediator)
    {
        _connectionFactory = connectionFactory;
    }
    
    private async Task<Profile> CreateProfile(CreateProfileDto createProfileDto)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();
        var comm = await SqlCommandGenerator.GenerateCommand(conn, "User/CreateProfile", new()
        {
            {"@userid", createProfileDto.UserId},
            {"@displayname", createProfileDto.DisplayName},
            {"@biography", createProfileDto.Biography}
        });

        await SqlChecks.ExecuteAndCheckIfSuccessful(comm);

        var profile = new Profile()
        {
            UserId = createProfileDto.UserId,
            DisplayName = createProfileDto.DisplayName,
            Biography = createProfileDto.Biography
        };

        return profile;
    }
    public async Task<User> CreateUser(CreateUserDto createUserDto)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();
        
        var passwordHashed = PasswordHasher.HashPassword(createUserDto.Password);
        
        var createUserComm = await SqlCommandGenerator.GenerateCommand(conn, "User/CreateUser", new()
        {
            {"@username", createUserDto.Username},
            {"@email", createUserDto.Email},
            {"@passwordhash", passwordHashed}
        });

        await SqlChecks.ExecuteAndCheckIfSuccessful(createUserComm);

        var userId = await GetLastId(conn);
        
        var createProfile = new CreateProfileDto()
        {
            UserId = userId,
            DisplayName = createUserDto.DisplayName,
            Biography = createUserDto.Biography
        };
        
        var profile = await CreateProfile(createProfile);

        var user = new User()
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            Id = userId,
            PasswordHash = passwordHashed,
            Profile = profile
        };
        
        user.AddDomainEvent(new UserCreatedEvent(user));

        await PublishMediatorEvents(user);

        return user;

    }

    public async Task<User?> GetUser(int userId)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();
        var comm = await SqlCommandGenerator.GenerateCommand(conn, "User/GetUser", new()
        {
            { "@userid", userId }
        });

        await using var reader = await comm.ExecuteReaderAsync();

        User? user = null;
        
        while (await reader.ReadAsync())
        {
            user = new()
            {
                Id = reader.GetInt32("id"),
                Email = reader.GetString("email"),
                PasswordHash = reader.GetString("passwordhash"),
                Username = reader.GetString("username"),
                Profile = null!
            };
        }

        return user;
    }
    
    public async Task<User?> GetUser(string username)
    {
        await using var conn = await _connectionFactory.CreateOpenConnection();
        var comm = await SqlCommandGenerator.GenerateCommand(conn, "User/GetUser", new()
        {
            { "@username", username }
        });

        await using var reader = await comm.ExecuteReaderAsync();

        User? user = null;
        
        while (await reader.ReadAsync())
        {
            user = new()
            {
                Id = reader.GetInt32("id"),
                Email = reader.GetString("email"),
                PasswordHash = reader.GetString("passwordhash"),
                Username = reader.GetString("username"),
                Profile = null!
            };
        }

        return user;
    }
}