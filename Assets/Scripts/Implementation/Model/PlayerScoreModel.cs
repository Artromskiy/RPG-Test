public class PlayerScoreModel : Model<IPlayerScoreModel>, IPlayerScoreModel
{
    private int _score;

    protected override string ModelKey { get; } = "PlayerScoreModel";

    public int Score
    {
        get => _score;
        set => SetScore(value);
    }

    private void SetScore(int score)
    {
        _score = score;
        InvokeModelChange();
    }
}
