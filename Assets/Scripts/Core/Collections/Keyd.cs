using System;

public abstract class Keyd<T> : IEquatable<Keyd<T>>, IKeyd<T> where T : IEquatable<T>
{
    public abstract T Key { get; }
    public bool Equals(Keyd<T> other) => Key.Equals(other.Key);
    public override bool Equals(object obj) => obj is Keyd<T> keyd && Equals(keyd);
    public override int GetHashCode() => Key.GetHashCode();
}