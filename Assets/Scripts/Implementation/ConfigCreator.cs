using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public static class ConfigCreator<T> where T : class, IConfig
{
    public static T Create(string key)
    {
        bool created = false;
        T config = null;
        var textAsset = Resources.Load<TextAsset>(key);
        if (textAsset != null)
        {
            //try
            {
                config = JsonConvert.DeserializeObject<T>(textAsset.text);
                created = true;
            }
            //catch (Exception e)
            //{
            //    throw new JsonSerializationException($"Config with value {key} not loaded", e);
            //}
        }
        if (!created)
            throw new FileNotFoundException("Config not found", key);
        return config;
    }
}
