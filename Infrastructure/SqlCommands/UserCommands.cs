using Domain.Entities;
using Infrastructure.DTO;
using Infrastructure.Helpers;

namespace Infrastructure.SqlCommands;

public class UserCommands : SqlCommandHelper
{
    private readonly SqlConnectionFactory _connectionFactory;
    public UserCommands(SqlConnectionFactory connectionFactory) : base(connectionFactory)
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
        
        await CreateProfile(createProfile);

        return new User()
        {
            Username = createUserDto.Username,
            Email = createUserDto.Email,
            Id = userId,
            PasswordHash = passwordHashed,
            Profile = new()
            {
                UserId = userId,
                DisplayName = createUserDto.DisplayName,
                Biography = createUserDto.Biography,
            }
        };

    }
}