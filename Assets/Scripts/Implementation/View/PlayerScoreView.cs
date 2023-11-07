using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreView : View<IPlayerScorePresenter>, IPlayerScoreView
{
    [SerializeField]
    private Button _earnScoreButton;
    [SerializeField]
    private TextMeshProUGUI _scoreTextCounter;

    private readonly GameEvent _onRequestEarn = new();
    IGameEvent IPlayerScoreView.OnRequestEarn => _onRequestEarn;

    protected override void Init()
    {
        _earnScoreButton.onClick.AddListener(_onRequestEarn.Invoke);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _earnScoreButton.onClick.RemoveListener(_onRequestEarn.Invoke);
        }
    }

    public int Score
    {
        set => UpdateScore(value);
    }

    private void UpdateScore(int score)
    {
        _scoreTextCounter.text = score.ToString();
    }
}
