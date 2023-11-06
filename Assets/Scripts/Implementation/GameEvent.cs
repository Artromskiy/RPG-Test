using System;

/// <summary>
/// Objects of this class encapsulate event
/// This is done to reduce creation of individual
/// Invoke methods inside user class,
/// but forces to provide explicit conversion to IGameEvent needed by interface
/// Note that object of this class shouldn't be transfered across app
/// </summary>
public sealed class GameEvent : IGameEvent
{
    public event Action Event;
    public void Invoke() => Event?.Invoke();
    public void Clear() => Event = null;
}

public sealed class GameEvent<T> : IGameEvent<T>
{
    public event Action<T> Event;
    public void Invoke(T value) => Event?.Invoke(value);
    public void Clear() => Event = null;
}
