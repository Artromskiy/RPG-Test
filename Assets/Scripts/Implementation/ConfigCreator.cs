using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class ConfigCreator<T> where T : class, IConfig, new()
{
    public static T Create(string key)
    {
        bool created = false;
        bool serializationError = false;
        T config = null;
        var textAsset = Resources.Load<TextAsset>(key);
        if (textAsset != null)
        {
            try
            {
                config = JsonConvert.DeserializeObject<T>(textAsset.text);
                created = true;
            }
            catch
            {
                serializationError = true;
            }
        }
        if (serializationError)
            throw new JsonSerializationException($"Config with key {key} not loaded");
        if (!created)
            throw new FileNotFoundException("Config not found", key);
        return config;
    }
}
