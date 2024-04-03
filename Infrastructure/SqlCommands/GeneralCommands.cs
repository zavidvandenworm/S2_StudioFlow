using System.Data;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using MySqlConnector;

namespace Infrastructure.SqlCommands;

public class GeneralCommands(SqlConnectionFactory connectionFactory)
{
    public async Task ExecuteAndCheckIfSuccessful(MySqlCommand command)
    {
        var affected = await command.ExecuteNonQueryAsync();

        if (affected == 0)
        {
            throw new Exception("Could not add user to project.");
        }
    }
    
    public async Task<int> GetLastId()
    {
        await using var conn = await connectionFactory.CreateOpenConnection();

        var comm = new MySqlCommand(await SqlScriptRetriever.GetScript("GetLastId"), conn);
        var id = await comm.ExecuteScalarAsync();

        return Convert.ToInt32(id);
    }
}