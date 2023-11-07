using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

/// <summary>
/// Immutable collection that stores objects and
/// infromation about existence of relationships
/// between objects
/// </summary>
/// <typeparam name="TValue"></typeparam>
[JsonObject(MemberSerialization.OptIn)] // This attribute forces user to specify serialized data with JsonProprtyAttribute
public class Graph<TKey, TValue> : IEnumerable<Graph<TKey, TValue>.Connection> where TValue : IKeyd<TKey> where TKey : IEquatable<TKey>
{
    [JsonProperty]
    private readonly TKey _root;
    [JsonProperty]
    private readonly Dictionary<TKey, Connection> _data;

    private (TKey element1, TKey element2)[] _connections;

    /// <summary>
    /// cached empty delegate
    /// </summary>
    private static readonly Predicate<TKey> _defaultSkipDelegate;

    /// <summary>
    /// Used for caching
    /// </summary>
    private static readonly HashSet<TKey> _searchedVertices = new();
    /// <summary>
    /// Used for caching
    /// </summary>
    private static readonly Queue<TKey> _searchQueue = new();

    public Connection this[TKey key] => _data[key];

    public TKey Root => _root;

    static Graph()
    {
        _defaultSkipDelegate = (_) => false;
    }

    [JsonConstructor]
    private Graph() { }
    public Graph(TKey root, Dictionary<TValue, HashSet<TValue>> data)
    {
        _root = root;
        _data = new();
        foreach (var item in data)
        {
            HashSet<TKey> connectionKeys = new();
            foreach (var connected in item.Value)
                connectionKeys.Add(connected.Key);
            _data.Add(item.Key.Key, new Connection(item.Key, connectionKeys));
        }
        if (!IsValid(out string message))
            throw new ArgumentException(message);
    }

    /// <summary>
    /// Returns Key to Key pairs of connected nodes
    /// </summary>
    /// <returns></returns>
    public ReadOnlySpan<(TKey element1, TKey element2)> GetConnections()
    {
        if (_connections == null)
        {
            HashSet<(TKey element1, TKey element2)> uniquePairs = new();
            foreach (var node in _data)
                foreach (var connected in node.Value)
                    uniquePairs.Add((node.Key, connected));
            _connections = new (TKey element1, TKey element2)[uniquePairs.Count];
            int indexer = 0;
            foreach (var item in uniquePairs)
                _connections[indexer++] = item;
        }
        return new(_connections);
    }

    /// <summary>
    /// Returns true if specified key is root of graph
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsRoot(TKey key) => _data.ContainsKey(key) && _root.Equals(key);

    /// <summary>
    /// Spicifies if graph contains node with input key
    /// </summary>
    /// <param name="key">Key to search</param>
    /// <returns></returns>
    public bool Contains(TKey key) => _data.ContainsKey(key);
    /// <summary>
    /// Spicifies if graph contains node
    /// </summary>
    /// <param name="value">Node to search</param>
    /// <returns></returns>
    public bool Contains(TValue value) => Contains(value.Key);


    public bool IsReachableFromRoot(TValue search, Predicate<TValue> skip = null) => IsReachableFromRoot(search.Key, x => skip(this[x].value));
    public bool IsReachableFromRoot(TKey search, Predicate<TValue> skip = null) => IsReachableFromRoot(search, x => skip(this[x].value));
    public bool IsReachableFromRoot(TValue search, Predicate<TKey> skip = null) => IsReachableFromRoot(search.Key, skip);
    /// <summary>
    /// Checks if element with specified value is still reachable from root
    /// even if some vertices will be skipped during value
    /// </summary>
    /// <param name="search"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsReachableFromRoot(TKey search, Predicate<TKey> skip = null)
    {
        skip ??= _defaultSkipDelegate;

        if (!_data.ContainsKey(search))
            return false;

        if (search.Equals(_root))
            return true;

        _searchQueue.Clear();
        _searchedVertices.Clear();

        _searchQueue.Enqueue(_root);
        _searchedVertices.Add(_root);

        foreach (var vertex in _data)
            if (!vertex.Key.Equals(search) && skip(vertex.Key))
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

    public bool IsRootReachable(TValue search, Predicate<TValue> skip = null) => IsRootReachable(search.Key, x => skip(this[x].value));
    public bool IsRootReachable(TValue from, Predicate<TKey> skip = null) => IsRootReachable(from.Key, skip);

    /// <summary>
    /// Checks if element is still reachable from root
    /// even if some vertices will be skipped during search
    /// </summary>
    /// <param name="from"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsRootReachable(TKey from, Predicate<TKey> skip = null)
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


    public bool IsRootReachable(TValue search, TValue skip) => IsRootReachable(search.Key, skip.Key);
    public bool IsRootReachable(TValue from, TKey skip) => IsRootReachable(from.Key, skip);
    public bool IsRootReachable(TKey from, TValue skip) => IsRootReachable(from, skip.Key);


    /// <summary>
    /// Checks if root is still reachable from specified node
    /// even if some vertices will be skipped during search
    /// </summary>
    /// <param name="from"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsRootReachable(TKey from, TKey skip)
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


    public bool IsReachableFromRoot(TValue search, TValue skip) => IsReachableFromRoot(search.Key, skip.Key);
    public bool IsReachableFromRoot(TValue search, TKey skip) => IsReachableFromRoot(search.Key, skip);
    public bool IsReachableFromRoot(TKey search, TValue skip) => IsReachableFromRoot(search, skip);

    /// <summary>
    /// Checks if specified node is still reachable from root
    /// even if some vertices will be skipped during search
    /// </summary>
    /// <param name="from"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public bool IsReachableFromRoot(TKey search, TKey skip)
    {
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

    /// <summary>
    /// Connection class that stores node and it's connections
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public readonly struct Connection : IEnumerable<TKey>
    {
        [JsonProperty]
        public readonly TValue value;
        [JsonProperty]
        private readonly HashSet<TKey> _hashset;
        public IEnumerator<TKey> GetEnumerator() => _hashset.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(TKey value) => _hashset.Contains(value);
        public bool Contains(TValue value) => _hashset.Contains(value.Key);

        public Connection(TValue value, HashSet<TKey> hashSet)
        {
            Debug.Assert(hashSet != null);
            this.value = value;
            _hashset = hashSet;
        }
    }
    public IEnumerator<Connection> GetEnumerator() => _data.Values.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

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
        if (_root == null || !_data.ContainsKey(_root))
        {
            message = "Graph does not contain root";
            return false;
        }
        foreach (var relations in _data)
        {
            if (relations.Value.Contains(relations.Key))
            {
                message = $"Vertex can not be self referenced. Vertex: {relations.Key}";
                return false;
            }
            foreach (var vertex in relations.Value)
            {
                if (!_data.TryGetValue(vertex, out var inverseRelation) || !inverseRelation.Contains(relations.Key))
                {
                    message = $"Directed vertices found in graph. Vertex: {vertex}";
                    return false;
                }
            }
        }
        HashSet<TKey> notCheckedVertices = new();
        foreach (var relations in _data)
            notCheckedVertices.UnionWith(relations.Value);
        notCheckedVertices.Remove(_root);
        Queue<TKey> searchQueue = new();
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