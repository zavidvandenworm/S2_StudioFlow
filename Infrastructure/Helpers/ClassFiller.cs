using MySqlConnector;

namespace Infrastructure.Helpers;

public static class ClassFiller
{
    public static T FillClass<T>(IDictionary<string, object> parameters)
    {
        var type = typeof(T);
        var instance = Activator.CreateInstance<T>();

        var typeProperties = type.GetProperties();

        foreach (KeyValuePair<string, object> pair in parameters)
        {
            var prop = typeProperties.FirstOrDefault(prop => prop.Name.ToLower() == pair.Key);
            if (prop is null)
            {
                continue;
            }

            prop.SetValue(instance, pair.Value);
        }

        return instance;
    }

    public static List<T> ReaderPopulateObject<T>(MySqlDataReader reader)
    {
        List<T> result = new List<T>();

        while (reader.Read())
        {
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                Console.WriteLine($"readerpopulate: {reader.GetName(i)} -- {reader.GetValue(i)}");
                parameters[reader.GetName(i)] = reader.GetValue(i);
            }

            result.Add(FillClass<T>(parameters));
        }

        reader.Close();

        return result;
    }
}