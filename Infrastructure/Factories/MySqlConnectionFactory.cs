using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Factories;

public class MySqlConnectionFactory
{
    private ConfigurationManager _configuration;
    
    public MySqlConnectionFactory(ConfigurationManager configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<MySqlConnection> CreateMysqlConnection()
    {
        var connection = new MySqlConnection(_configuration.GetConnectionString("studioflow"));
        await connection.OpenAsync();

        return connection;
    }
}