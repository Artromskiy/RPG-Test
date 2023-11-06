using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

[JsonObject(MemberSerialization.OptIn)]
public abstract class Model<T> : IModel<T> where T : class, IModel<T>
{
    private static readonly JsonSerializer _jsonSerializer = JsonSerializer.CreateDefault();
    public event Action<T> OnModelChanged;
    private string _modelKey;

    string IModel.ModelKey { set => _modelKey = value; }

    /// <summary>
    /// Saves object data into file with specified ModelKey
    /// </summary>
    private void Save()
    {
        using StreamWriter file = File.CreateText(_modelKey);
        using JsonTextWriter writer = new(file);
        _jsonSerializer.Serialize(writer, this);
        //writer.Flush(); // Flush called automatically on stream close
    }

    /// <summary>
    /// Saves object data and notifies subscribers about it
    /// </summary>
    protected void InvokeModelChange()
    {
        Save();
        Debug.Assert(this is T);
        OnModelChanged.Invoke(this as T);
    }
}
