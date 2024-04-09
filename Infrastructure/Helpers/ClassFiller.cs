namespace Infrastructure.Helpers;

public static class ClassFiller
{
    public static T FillClass<T>(IDictionary<string, object> parameters)
    {
        var type = typeof(T);
        var instance = Activator.CreateInstance<T>();

        foreach (KeyValuePair<string, object> pair in parameters)
        {
            var prop = type.GetProperty(pair.Key);
            if (prop is null)
            {
                continue;
            }
            prop.SetValue(instance, pair.Value);
        }

        return instance;
    }
}