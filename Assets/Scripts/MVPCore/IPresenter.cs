using System;

public interface IPresenter<TView, TPresenter>: IDisposable
    where TView: IView
    where TPresenter : IPresenter<TView, TPresenter>
    
{
    TView View { get; set; }
}
