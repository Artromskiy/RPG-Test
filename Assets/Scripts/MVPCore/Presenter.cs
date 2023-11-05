using Reflex.Attributes;
using System;

public abstract class Presenter<TView, TPresenter> : IPresenter<TView, TPresenter>
    where TView : IView
    where TPresenter : class, IPresenter<TView, TPresenter>
{
    [Inject]
    public TView View { get; set; }

    private bool _disposed;

    ~Presenter()
    {
        if (!_disposed)
            Dispose(false);
    }

    public void Dispose()
    {
        if(!_disposed)
            Dispose(true);
    }

    /// <summary>
    /// Dispose method for inheritors
    /// Note that _disposed boolean should force
    /// implementors to call base dispose at the end
    /// This should reduce potential errors
    /// </summary>
    /// <param name="disposing">True when called from user code, False when called from destructor
    /// All unmanaged resources should be cleared despite it's called from destructor or user code
    /// All managed resources should be cleared if called from user code</param>
    protected virtual void Dispose(bool disposing)
    {
        if(disposing)
            View?.Dispose();

        _disposed = true;
    }
}