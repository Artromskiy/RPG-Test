using System;
using System.Collections.Generic;

/// <summary>
/// Immutable collection that stores objects and
/// infromation about existence of relationships
/// between objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class SkillGraph<T>
{
    private readonly T _root;
    private readonly Dictionary<T, HashSet<T>> _data;

    private static readonly Predicate<T> _defaultSkipDelegate;
    static SkillGraph()
    {
        _defaultSkipDelegate = (_) => false;
    }

    /// <summary>
    /// Checks if element is still reachable from root
    /// even if some vertices will be skipped during search
    /// </summary>
    /// <param name="search"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsReachable(T search, Predicate<T> skip = null)
    {
        skip ??= _defaultSkipDelegate;

        if (skip(search))
            return false;

        if (!_data.ContainsKey(search))
            return false;

        if (search.Equals(_root))
            return true;

        Queue<T> searchQueue = new();
        HashSet<T> checkedVertices = new();

        searchQueue.Enqueue(_root);
        checkedVertices.Add(_root);

        foreach (var vertex in _data)
            if (skip(vertex.Key))
                checkedVertices.Add(vertex.Key);

        while (searchQueue.Count != 0)
        {
            var current = searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (!checkedVertices.Contains(item))
                {
                    if (item.Equals(search))
                        return true;

                    searchQueue.Enqueue(item);
                    checkedVertices.Add(item);
                }
            }
        }
        return false;
    }
    public bool IsReachable(T search, T skip)
    {
        if (skip.Equals(search))
            return false;

        if (!_data.ContainsKey(search))
            return false;

        if (search.Equals(_root))
            return true;

        Queue<T> searchQueue = new();
        HashSet<T> checkedVertices = new();

        searchQueue.Enqueue(_root);
        checkedVertices.Add(_root);

        checkedVertices.Add(skip);

        while (searchQueue.Count != 0)
        {
            var current = searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (!checkedVertices.Contains(item))
                {
                    if (item.Equals(search))
                        return true;

                    searchQueue.Enqueue(item);
                    checkedVertices.Add(item);
                }
            }
        }
        return false;
    }

    public bool IsAllReachable(HashSet<T> search, Predicate<T> skip)
    {
        foreach (var element in search)
            if (!IsReachable(element, skip))
                return false;
        return true;
    }

    public bool IsAllReachable(HashSet<T> search, T skip)
    {
        foreach (var element in search)
            if (!IsReachable(element, skip))
                return false;
        return true;
    }

    public HashSet<T>? GetNear(T search)
    {
        if (_data.TryGetValue(search, out var result))
            return result;
        return null;
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
        if (!_data.ContainsKey(_root))
        {
            message = "Graph does not contain root";
            return false;
        }
        foreach (var relations in _data)
        {
            if (relations.Value.Contains(relations.Key))
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
        while (searchQueue.Count != 0)
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
        if (notCheckedVertices.Count > 0)
        {
            message = "Not all vertices reachable from root";
            return false;
        }
        return true;
    }
}