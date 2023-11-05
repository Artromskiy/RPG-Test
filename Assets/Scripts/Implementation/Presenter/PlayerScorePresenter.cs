using Reflex.Attributes;

public class PlayerScorePresenter : Presenter<IPlayerScoreView, IPlayerScorePresenter>, IPlayerScorePresenter
{
    [Inject]
    private readonly IPlayerScoreModel _scoreModel;

    public PlayerScorePresenter()
    {
        View.OnRequestEarn += Earn;
        _scoreModel.OnModelChanged += OnScoreChanged;
    }

    private void OnScoreChanged(IPlayerScoreModel playerScoreModel)
    {
        View.Score = playerScoreModel.Score;
    }

    private void Earn()
    {
        _scoreModel.Score++;
    }
}
