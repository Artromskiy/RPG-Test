using System;

/// <summary>
/// Provides access to event
/// </summary>
public interface IGameEvent
{
    public event Action Event;
}
/// <summary>
/// provides access to event
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IGameEvent<T>
{
    public event Action<T> Event;
}