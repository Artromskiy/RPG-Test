using System;
using System.Collections.Generic;

/// <summary>
/// Simple reactive field that executes events in FIFO order
/// </summary>
/// <typeparam name="T"></typeparam>
public class ReactiveField<T> : IReactiveField<T>
{
    private readonly List<Action<T>> _listeners = new();

    private Queue<QueuedAction> _executionQueue;
    private Queue<QueuedAction> Queued => _executionQueue ??= new Queue<QueuedAction>(); // init only if needed
    /// <summary>
    /// Comparer used for Value types, as we don't want to receive same data if value not changed
    /// </summary>
    private static readonly EqualityComparer<T> _comparer = typeof(T).IsValueType ? EqualityComparer<T>.Default : null;

    public event Action<T> Event
    {
        add => AddListener(value);
        remove => RemoveListener(value);
    }

    private T _value;
    private bool _invoking;

    public T Value
    {
        get => _value;
        set => Invoke(value);
    }

    public ReactiveField(T defaultValue = default)
    {
        _value = defaultValue;
    }


    /// <summary>
    /// Adds listener to list or adds to execution queue
    /// </summary>
    /// <param name="listener"></param>
    private void AddListener(Action<T> listener)
    {
        if (!_invoking)
            _listeners.Add(listener);
        else
            Queued.Enqueue(QueuedAction.AddAction(listener));
    }

    /// <summary>
    /// Removes listener from list or adds to execution queue
    /// </summary>
    /// <param name="listener"></param>
    private void RemoveListener(Action<T> listener)
    {
        if (!_invoking)
            _listeners.Remove(listener);
        else
            Queued.Enqueue(QueuedAction.RemoveAction(listener));
    }


    public void Invoke(T data)
    {
        if (_invoking)
        {
            Queued.Enqueue(QueuedAction.InvokeAction(data));
            return;
        }
        if (_comparer != null && _comparer.Equals(data, _value)) // Equality comparer used to avoid boxing
            return;

        _invoking = true;
        _value = data;
        InvokeInternal();
        _invoking = false;

        while (_executionQueue != null && _executionQueue.Count > 0)
        {
            var action = _executionQueue.Dequeue();
            if (action.actionType == ActionType.Invoke)
                Invoke(action.value);
            else if (action.actionType == ActionType.Add)
                AddListener(action.action);
            else
                RemoveListener(action.action);
        }
    }

    private void InvokeInternal()
    {
        var data = _value;
        foreach (var listener in _listeners)
            listener.Invoke(data);
    }


    /// <summary>
    /// Struct for saving queued operations
    /// </summary>
    private readonly struct QueuedAction
    {
        public readonly ActionType actionType;
        public readonly Action<T> action;
        public readonly T value;

        private QueuedAction(ActionType actionType, Action<T> action, T value)
        {
            this.actionType = actionType;
            this.action = action;
            this.value = value;
        }
        public static QueuedAction AddAction(Action<T> action) => new(ActionType.Add, action, default);
        public static QueuedAction RemoveAction(Action<T> action) => new(ActionType.Remove, action, default);
        public static QueuedAction InvokeAction(T value) => new(ActionType.Invoke, default, value);
    }
    private enum ActionType
    {
        Invoke,
        Remove,
        Add,
    }
}
