using System;
using System.Collections.Generic;

public class ReactiveField<T> : IReactiveField<T>
{
    private static readonly EqualityComparer<T> _comparer = typeof(T).IsValueType ? EqualityComparer<T>.Default : null;

    public event Action<T> Event;

    private T _value;

    public T Value
    {
        get => _value;
        set => Invoke(value);
    }

    public ReactiveField(T defaultValue = default)
    {
        _value = defaultValue;
    }

    public void Invoke(T value)
    {
        if (_comparer == null || !_comparer.Equals(value, _value))
        {
            _value = value;
            Event.Invoke(_value);
        }
    }
}
