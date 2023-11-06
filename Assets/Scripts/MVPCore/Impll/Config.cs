using Newtonsoft.Json;
using System.IO;

public abstract class Config<T> : IConfig<T>
{
    protected abstract string ConfigKey { get; }
    private static readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();
    public T ConfigData { get; }

    public Config()
    {
        using StreamReader file = File.OpenText(ConfigKey);
        using JsonTextReader reader = new(file);
        ConfigData = _jsonSerializer.Deserialize<T>(reader);
    }
}
