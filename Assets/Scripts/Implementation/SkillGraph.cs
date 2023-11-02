using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

/// <summary>
/// Immutable collection that stores objects and
/// infromation about existence of relationships
/// between objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class SkillGraph<T> : ISerializationCallbackReceiver
{
    private T _root;
    private Dictionary<T, HashSet<T>> _data;

    private T _serializedRoot;
    private readonly List<KeydList> _serializedData;

    public void OnAfterDeserialize()
    {
        _root = _serializedRoot;
        _data = new();
        foreach (var item in _serializedData)
            _data.Add(item.key, new(item.values));
        if(!IsValid(out var message))
            throw new SerializationException(message);
    }

    public void OnBeforeSerialize()
    {
        _serializedRoot = _root;
        _serializedData.Clear();
        foreach (var item in _data)
            _serializedData.Add(new(item.Key, new(item.Value)));
        if (!IsValid(out var message))
            throw new SerializationException(message);
    }

    [Serializable]
    private struct KeydList
    {
        public T key;
        public List<T> values;

        public KeydList(T key, List<T> values)
        {
            this.key = key;
            this.values = values;
        }
    }

    /// <summary>
    /// Checks the correctness of the graph.
    /// The root must be an element of the graph.
    /// No self referencing of nodes
    /// The graph must be undirected
    /// All vertices must be reachable from any vertex.
    /// </summary>
    /// <returns></returns>
    private bool IsValid(out string message)
    {
        message = string.Empty;
        if(!_data.ContainsKey(_root))
        {
            message = "Graph does not contain root";
            return false;
        }
        foreach (var relations in _data)
        {
            if(relations.Value.Contains(relations.Key))
            {
                message = "Vertex can not be self referenced";
                return false;
            }
            foreach (var vertex in relations.Value)
            {
                if (!_data.TryGetValue(vertex, out var inverseRelation) || !inverseRelation.Contains(vertex))
                {
                    message = "Directed vertices found in graph";
                    return false;
                }
            }
        }
        HashSet<T> notCheckedVertices = new();
        foreach (var relations in _data)
            notCheckedVertices.UnionWith(relations.Value);
        notCheckedVertices.Remove(_root);
        Queue<T> searchQueue = new();
        searchQueue.Enqueue(_root);
        while(searchQueue.Count != 0)
        {
            var current = searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (notCheckedVertices.Contains(item))
                {
                    searchQueue.Enqueue(item);
                    notCheckedVertices.Remove(item);
                }
            }
        }
        if(notCheckedVertices.Count > 0)
        {
            message = "Not all vertices reachable from root";
            return false;
        }
        return true;
    }
}