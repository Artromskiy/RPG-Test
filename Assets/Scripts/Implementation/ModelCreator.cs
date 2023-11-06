using Newtonsoft.Json;
using System.IO;


public static class ModelCreator<T> where T : class, IModel, new()
{
    private static readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();
    public static T Create(string key)
    {
        bool created = false;
        T model = null;
        try
        {
            if (File.Exists(key))
            {
                using StreamReader file = File.OpenText(key);
                using JsonTextReader reader = new(file);
                model = _jsonSerializer.Deserialize<T>(reader);
                created = true;
            }
        }
        catch { }
        if (!created)
            model = new();
        model.ModelKey = key;
        return model;
    }
}
