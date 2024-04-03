using Infrastructure.Helpers;
using MySqlConnector;

namespace Infrastructure.SqlCommands;

public class SqlCommandHelper(SqlConnectionFactory connectionFactory)
{
    protected async Task<int> GetLastId(MySqlConnection conn)
    {
        var comm = new MySqlCommand(await SqlScriptRetriever.GetScript("GetLastId"), conn);
        var id = await comm.ExecuteScalarAsync();

        return Convert.ToInt32(id);
    }
}