using System.Reflection;
using MySqlConnector;

namespace Infrastructure.Helpers;

public static class SqlScriptRetriever
{
    public static async Task<string> GetScript(string scriptName)
    {
        string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, @"SQL\" + scriptName + ".sql");
        var script = await File.ReadAllTextAsync(path);

        return script;
    }
}