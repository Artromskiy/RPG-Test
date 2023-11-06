using Newtonsoft.Json;

public class PlayerScoreModel : Model<IPlayerScoreModel>, IPlayerScoreModel
{
    [JsonProperty]
    private int _score;

    public PlayerScoreModel()
    {
        _score = 0;
    }
    [JsonIgnore]
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
