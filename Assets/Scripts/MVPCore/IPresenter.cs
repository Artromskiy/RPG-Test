using System;

public interface IPresenter<TView> : IDisposable
    where TView : class, IView
{
    TView View { get; }
}
