using Newtonsoft.Json;
using System.IO;

public static class ModelCreator<T> where T : class, IModel, new()
{
    private static readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();
    private const string ModelFolder = "Models";
    private static readonly string ModelFolderPath;

    static ModelCreator()
    {
        ModelFolderPath = Path.Combine(UnityEngine.Application.persistentDataPath, ModelFolder);
        if (!Directory.Exists(ModelFolderPath))
            Directory.CreateDirectory(ModelFolderPath);
    }

    public static T Create(string key)
    {
        bool created = false;
        T model = null;
        string filePath = Path.Combine(ModelFolderPath, key);
        try
        {
            if (File.Exists(filePath))
            {
                using StreamReader file = File.OpenText(filePath);
                using JsonTextReader reader = new(file);
                model = _jsonSerializer.Deserialize<T>(reader);
                model.ModelKey = filePath;
                created = true;
            }
        }
        catch { }
        if (!created)
        {
            model = new();
            model.ModelKey = filePath;
            model.Save();
        }
        return model;
    }
}
