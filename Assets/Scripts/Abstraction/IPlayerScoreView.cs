using System;
public interface IPlayerScoreView : IView
{
    public event Action OnRequestEarn;
    public int Score { set; }
}