using Reflex.Attributes;
using System;
using UnityEngine;

/// <summary>
/// Base View class providing dispose callbacks
/// </summary>
public abstract class View<TPresenter> : MonoBehaviour, IView
{
    private bool _disposed;
    protected TPresenter Presenter { get; private set; }
    [Inject]
    public void Init(TPresenter presenter)
    {
        Presenter = presenter;
        Init();
    }
    protected abstract void Init();

    ~View()
    {
        if (!_disposed)
            Dispose(false);
    }
    /// <summary>
    /// Protected used to force using of Dispose pattern
    /// </summary>
    protected void OnDestroy()
    {
        if (!_disposed)
            Dispose(false);
    }

    /// <summary>
    /// Provides clearing functionality for resources
    /// Note that it created explicitly,
    /// this is done to remove accidential call from inheritors
    void IDisposable.Dispose()
    {
        if (!_disposed)
            Dispose(true);
    }

    /// <summary>
    /// Dispose method for inheritors
    /// Note that _disposed boolean should force
    /// implementors to call base dispose at the end
    /// This should reduce potential errors
    /// </summary>
    /// <param name="disposing">True when called from user code, False when called from destructor or OnDestroy</param>
    protected virtual void Dispose(bool disposing)
    {
        // As unity manages objects in it's own manner we will call Destroy only on user code
        // If we will call it from destructor - gameObject sended to Destroy will be null and Unity will throw exception
        if (disposing)
            Destroy(gameObject);
        _disposed = true;
    }
}
