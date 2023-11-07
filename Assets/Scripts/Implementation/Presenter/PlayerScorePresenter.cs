public class PlayerScorePresenter : Presenter<IPlayerScoreView>, IPlayerScorePresenter
{
    private readonly IPlayerScoreModel _scoreModel;

    public PlayerScorePresenter(IPlayerScoreModel scoreModel, IPlayerScoreView view) :base(view)
    {
        _scoreModel = scoreModel;

        OnScoreChanged(_scoreModel);
        View.OnRequestEarn.Event += Earn;
        _scoreModel.OnModelChanged.Event += OnScoreChanged;
    }

    protected override void Dispose(bool disposing)
    {
        if (_scoreModel != null)
            _scoreModel.OnModelChanged.Event -= OnScoreChanged;
        if(View != null)
            View.OnRequestEarn.Event += Earn;

        base.Dispose(disposing);
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
