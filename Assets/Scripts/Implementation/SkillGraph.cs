using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Immutable collection that stores objects and
/// infromation about existence of relationships
/// between objects
/// </summary>
/// <typeparam name="T"></typeparam>
public class SkillGraph<T> : IEnumerable<T>
{
    private readonly T _root;
    private readonly Dictionary<T, HashSet<T>> _data;

    private static readonly Predicate<T> _defaultSkipDelegate;

    private static readonly HashSet<T> _searchedVertices = new();
    private static readonly Queue<T> _searchQueue = new();

    static SkillGraph()
    {
        _defaultSkipDelegate = (_) => false;
    }

    /// <summary>
    /// Checks if element is still reachable from root
    /// even if some vertices will be skipped during from
    /// </summary>
    /// <param name="search"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsReachableFromRoot(T search, Predicate<T> skip = null)
    {
        skip ??= _defaultSkipDelegate;

        if (skip(search))
            return false;

        if (!_data.ContainsKey(search))
            return false;

        if (search.Equals(_root))
            return true;

        _searchQueue.Clear();
        _searchedVertices.Clear();

        _searchQueue.Enqueue(_root);
        _searchedVertices.Add(_root);

        foreach (var vertex in _data)
            if (skip(vertex.Key))
                _searchedVertices.Add(vertex.Key);

        while (_searchQueue.Count != 0)
        {
            var current = _searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (!_searchedVertices.Contains(item))
                {
                    if (item.Equals(search))
                        return true;

                    _searchQueue.Enqueue(item);
                    _searchedVertices.Add(item);
                }
            }
        }
        return false;
    }

    /// <summary>
    /// Checks if element is still reachable from root
    /// even if some vertices will be skipped during from
    /// </summary>
    /// <param name="from"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsRootReachable(T from, Predicate<T> skip = null)
    {
        skip ??= _defaultSkipDelegate;

        if (skip(from))
            return false;

        if (!_data.ContainsKey(from))
            return false;

        if (from.Equals(_root))
            return true;

        _searchQueue.Clear();
        _searchedVertices.Clear();

        _searchQueue.Enqueue(from);
        _searchedVertices.Add(from);

        foreach (var vertex in _data)
            if (skip(vertex.Key))
                _searchedVertices.Add(vertex.Key);

        while (_searchQueue.Count != 0)
        {
            var current = _searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (!_searchedVertices.Contains(item))
                {
                    if (item.Equals(_root))
                        return true;

                    _searchQueue.Enqueue(item);
                    _searchedVertices.Add(item);
                }
            }
        }
        return false;
    }


    public bool IsRootReachable(T from, T skip)
    {
        if (skip.Equals(from))
            return false;

        if (!_data.ContainsKey(from))
            return false;

        if (from.Equals(_root))
            return true;

        _searchQueue.Clear();
        _searchedVertices.Clear();

        _searchQueue.Enqueue(from);
        _searchedVertices.Add(from);
        _searchedVertices.Add(skip);

        while (_searchQueue.Count != 0)
        {
            var current = _searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (!_searchedVertices.Contains(item))
                {
                    if (item.Equals(_root))
                        return true;

                    _searchQueue.Enqueue(item);
                    _searchedVertices.Add(item);
                }
            }
        }
        return false;
    }


    public bool IsReachableFromRoot(T search, T skip)
    {
        if (skip.Equals(search))
            return false;

        if (!_data.ContainsKey(search))
            return false;

        if (search.Equals(_root))
            return true;

        _searchQueue.Clear();
        _searchedVertices.Clear();

        _searchQueue.Enqueue(_root);
        _searchedVertices.Add(_root);

        _searchedVertices.Add(skip);

        while (_searchQueue.Count != 0)
        {
            var current = _searchQueue.Dequeue();
            foreach (var item in _data[current])
            {
                if (!_searchedVertices.Contains(item))
                {
                    if (item.Equals(search))
                        return true;

                    _searchQueue.Enqueue(item);
                    _searchedVertices.Add(item);
                }
            }
        }
        return false;
    }

    public bool IsAllReachable(HashSet<T> search, Predicate<T> skip)
    {
        foreach (var element in search)
            if (!IsReachableFromRoot(element, skip))
                return false;
        return true;
    }

    public bool IsAllReachable(HashSet<T> search, T skip)
    {
        foreach (var element in search)
            if (!IsReachableFromRoot(element, skip))
                return false;
        return true;
    }

    public HashSet<T> GetConnections(T search)
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

    public readonly struct ConnectionEnumerator : IEnumerable<T>
    {
        private readonly HashSet<T> _enumerator;
        public IEnumerator<T> GetEnumerator() => _enumerator.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public ConnectionEnumerator(HashSet<T> hashSet)
        {
            Debug.Assert(hashSet != null);
            _enumerator = hashSet;
        }
    }


    public ConnectionEnumerator EnumerateConnections(T key)
    {
        if (!_data.ContainsKey(key))
            throw new ArgumentException(nameof(key));
        return new(_data[key]);
    }

    public IEnumerator<T> GetEnumerator() => _data.Keys.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}