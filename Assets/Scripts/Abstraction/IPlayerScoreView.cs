using System;
public interface IPlayerScoreView : IView
{
    public IGameEvent OnRequestEarn { get; }
    public int Score { set; }
}