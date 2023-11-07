public class PlayerScorePresenter : Presenter<IPlayerScoreView>, IPlayerScorePresenter
{
    private readonly IPlayerScoreModel _scoreModel;

    private readonly ReactiveField<int> _playerScore = new();
    IReactiveField<int> IPlayerScorePresenter.Score => _playerScore;

    public PlayerScorePresenter(IPlayerScoreModel scoreModel, IPlayerScoreView view) :base(view)
    {
        _scoreModel = scoreModel;

        View.OnRequestEarn.Event += Earn;
        View.OnRequestLoseAll.Event += LoseAll;

        _scoreModel.OnScoreChanged.Event += _playerScore.Invoke;

        _playerScore.Value = _scoreModel.Score;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            View.OnRequestEarn.Event -= Earn;
            View.OnRequestLoseAll.Event -= LoseAll;

            _scoreModel.OnScoreChanged.Event -= _playerScore.Invoke;
        }

        base.Dispose(disposing);
    }

    private void Earn()
    {
        _scoreModel.Score++;
    }
    private void LoseAll()
    {
        _scoreModel.Score = 0;
    }
}
