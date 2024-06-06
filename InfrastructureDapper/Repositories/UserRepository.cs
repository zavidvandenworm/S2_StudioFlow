using System.Data;
using Dapper;
using Domain.DTO;
using Domain.Entities;
using InfrastructureDapper.Helpers;
using InfrastructureDapper.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace InfrastructureDapper.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _dbConnection;

    public UserRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }
    
    public async Task<User?> GetById(int userId)
    {
        const string sql = @"SELECT * FROM users WHERE id = @userid";

        var parameters = new DynamicParameters();
        parameters.Add("@userid", userId);

        var result = (await _dbConnection.QueryAsync<User>(sql, parameters)).ToList();

        return result.FirstOrDefault();
    }

    public async Task<User?> GetByUsername(string username)
    {
        const string sql = @"SELECT * FROM users WHERE username = @username";
        
        var parameters = new DynamicParameters();
        parameters.Add("@username", username);

        var user = await _dbConnection.QueryAsync<User>(sql, parameters);

        return user.FirstOrDefault();
    }

    public async Task<int?> Create(CreateUserDto createUserDto)
    {
        const string sql = @"INSERT INTO users (`id`, `username`, `email`, `passwordHash`) VALUES (null, @username, @email, @passwordhash); SELECT LAST_INSERT_ID();";
        
        var parameters = new DynamicParameters();
        parameters.Add("@username", createUserDto.Username);
        parameters.Add("@email", createUserDto.Email);
        parameters.Add("@passwordhash", PasswordHasher.HashPassword(createUserDto.Password));
        
        var userid = await _dbConnection.ExecuteScalarAsync(sql, parameters);

        if (userid is null)
        {
            return null;
        }

        var useridInt = Convert.ToInt32(userid);

        await AddUserProfile(useridInt, createUserDto.DisplayName, createUserDto.Biography,
            "https://images.pexels.com/photos/771742/pexels-photo-771742.jpeg?auto=compress&cs=tinysrgb&dpr=1&w=500");

        return useridInt;
    }

    public async Task<Profile> GetUserProfile(int userId)
    {
        const string sql = @"SELECT * FROM profiles WHERE userId = @userid";

        var parameters = new DynamicParameters();
        parameters.Add("@userid", userId);

        var profile = await  _dbConnection.QuerySingleAsync<Profile>(sql, parameters);
        return profile;
    }

    public async Task UpdateUserProfile(int userId, Profile profile)
    {
        const string sql = @"UPDATE profiles SET displayName = @displayname, profilepicture = @profilepicture";
        var parameters = new DynamicParameters();
        parameters.Add("@displayname", profile.DisplayName);
        parameters.Add("@profilepicture", profile.ProfilePicture);

        await _dbConnection.ExecuteAsync(sql, parameters);
    }

    private async Task AddUserProfile(int userId, string displayname, string biography, string profilepicture)
    {
        const string sql = @"INSERT INTO profiles (`userId`, `displayName`, `biography`, `profilepicture`) VALUES (@userid, @displayname, @biography, @profilepicture)";
        
        var parameters = new DynamicParameters();
        parameters.Add("@userid", userId);
        parameters.Add("@displayname", displayname);
        parameters.Add("@biography", biography);
        parameters.Add("@profilepicture", profilepicture);
        
        await _dbConnection.ExecuteAsync(sql, parameters);
    }
}