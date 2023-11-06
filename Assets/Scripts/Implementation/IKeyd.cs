using System;

public interface IKeyd<TKey> where TKey : IEquatable<TKey>
{
    public TKey Key { get; }
}
