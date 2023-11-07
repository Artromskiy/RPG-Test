using System;

public abstract class Presenter<TView> : IPresenter<TView>
    where TView : class, IView
{
    public TView View { get; private set; }

    private bool _disposed;

    ~Presenter()
    {
        if (!_disposed)
            Dispose(false);
    }

    protected Presenter(TView view)
    {
        View = view;
    }

    /// <summary>
    /// Provides clearing functionality for resources
    /// Note that it created explicitly,
    /// this is done to remove accidential call from inheritors
    /// </summary>
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
    /// All unmanaged resources should be cleared despite it's called from destructor or user code
    /// All managed resources should be cleared if called from user code</param>
    /// <param name="disposing">True when called from user code, False when called from destructor
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            View?.Dispose();

        _disposed = true;
    }
}