using MySqlConnector;

namespace Infrastructure.Helpers;

public static class SqlChecks
{
    public static async Task ExecuteAndCheckIfSuccessful(MySqlCommand command)
    {
        var affected = await command.ExecuteNonQueryAsync();

        if (affected == 0)
        {
            throw new Exception("Could not add user to project.");
        }
    }
}