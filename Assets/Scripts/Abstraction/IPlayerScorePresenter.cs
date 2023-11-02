using System;

public interface IPlayerScorePresenter : IPresenter<IPlayerScoreView, IPlayerScorePresenter>
{
    public int Score { get; }
    public event Action<int> OnScoreChanged;
}
