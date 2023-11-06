using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

public abstract class Model<T> : IModel<T> where T : class, IModel<T>
{
    public event Action<T> OnModelChanged;
    protected abstract string ModelKey { get; }
    private static readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();


    private void Save()
    {
        using StreamWriter file = File.CreateText(ModelKey);
        using JsonTextWriter writer = new(file);
        _jsonSerializer.Serialize(writer, this);
    }

    protected void InvokeModelChange()
    {
        Save();
        Debug.Assert(this is T);
        OnModelChanged.Invoke(this as T);
    }
}
