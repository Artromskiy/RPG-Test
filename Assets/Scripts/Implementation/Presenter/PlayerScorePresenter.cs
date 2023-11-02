using Reflex.Attributes;
using System;

public class PlayerScorePresenter : Presenter<IPlayerScoreView, IPlayerScorePresenter>, IPlayerScorePresenter
{
    [Inject]
    private readonly IPlayerScoreModel _scoreModel;

    public PlayerScorePresenter()
    {
        View.OnRequestEarn += Earn;
        _scoreModel.OnScoreChanged += OnScoreChanged;
    }

    public int Score => _scoreModel.Score;
    public event Action<int> OnScoreChanged;

    private void Earn()
    {
        _scoreModel.Score++;
    }
}
