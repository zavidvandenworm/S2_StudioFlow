using MySqlConnector;

namespace Infrastructure.Helpers;

public static class SqlCommandGenerator
{
    public static async Task<MySqlCommand> GenerateCommand(MySqlConnection connection, string scriptName, Dictionary<string, object> parameters)
    {
        var script = await SqlScriptRetriever.GetScript(scriptName);
        var command = new MySqlCommand(script, connection);

        foreach (KeyValuePair<string, object> pair in parameters)
        {
            command.Parameters.AddWithValue(pair.Key, pair.Value);
        }

        return command;
    }
}