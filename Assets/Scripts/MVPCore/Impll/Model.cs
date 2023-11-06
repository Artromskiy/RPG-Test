using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
/// <summary>
/// Base Model class implementing notification of it's changes
/// </summary>
/// <typeparam name="T"></typeparam>
[JsonObject(MemberSerialization.OptIn)] // This attribute forces user to specify serialized data with JsonProprtyAttribute
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
