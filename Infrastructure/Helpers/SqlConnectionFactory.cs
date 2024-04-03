using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace Infrastructure.Helpers;

public class SqlConnectionFactory
{
    private string _connectionString;
    
    public SqlConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("studioflow")!;
    }

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<MySqlConnection> CreateOpenConnection()
    {
        var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();

        return connection;
    } 
}