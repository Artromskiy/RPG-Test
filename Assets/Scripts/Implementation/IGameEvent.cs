using System;
public interface IGameEvent
{
    public event Action Event;
}

public interface IGameEvent<T>
{
    public event Action<T> Event;
}