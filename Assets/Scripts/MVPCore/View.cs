using UnityEngine;

public class View<TPresenter> : MonoBehaviour, IView<TPresenter>
{
    protected TPresenter _presenter;

    public TPresenter Presenter
    {
        get => _presenter;
        set => _presenter = value;
    }
}
