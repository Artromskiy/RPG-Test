using Newtonsoft.Json;

public class PlayerScoreModel : Model<IPlayerScoreModel>, IPlayerScoreModel
{
    [JsonProperty]
    private int _score;

    private readonly GameEvent<int> _onScoreChanged = new();
    public IGameEvent<int> OnScoreChanged => _onScoreChanged;

    public PlayerScoreModel()
    {
        _score = 0;
    }

    public int Score
    {
        get => _score;
        set => SetScore(value);
    }

    private void SetScore(int score)
    {
        _score = score;
        _onScoreChanged?.Invoke(_score);
        InvokeModelChange();
    }
}
