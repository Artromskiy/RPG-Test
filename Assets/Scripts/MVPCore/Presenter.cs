using Reflex.Attributes;

public abstract class Presenter<TView, TPresenter> : IPresenter<TView, TPresenter>
    where TView : IView<TPresenter>
    where TPresenter : class, IPresenter<TView, TPresenter>
{
    protected TView _view;


    [Inject]
    public TView View
    {
        get => _view;
        set
        {
            _view = value;
            _view.Presenter = this as TPresenter;
        }
    }
}