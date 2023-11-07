using System;

public interface IReactiveField<T>
{
    public event Action<T> Event;
    public T Value { get; }
}
