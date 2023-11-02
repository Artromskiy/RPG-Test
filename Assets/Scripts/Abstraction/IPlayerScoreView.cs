using System;
public interface IPlayerScoreView : IView<IPlayerScorePresenter>
{
    public event Action OnRequestEarn;
}