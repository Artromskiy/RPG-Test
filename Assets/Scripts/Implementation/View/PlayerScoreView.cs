using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreView : View, IPlayerScoreView
{
    [SerializeField]
    private Button _earnScoreButton;
    [SerializeField]
    private TextMeshProUGUI _scoreTextCounter;

    private readonly GameEvent _onRequestEarn = new();
    IGameEvent IPlayerScoreView.OnRequestEarn => _onRequestEarn;

    public int Score
    {
        set => UpdateScore(value);
    }

    private void OnEnable()
    {
        _earnScoreButton.onClick.AddListener(_onRequestEarn.Invoke);
    }

    private void OnDisable()
    {
        _earnScoreButton.onClick.RemoveListener(_onRequestEarn.Invoke);
    }

    private void UpdateScore(int score)
    {
        _scoreTextCounter.text = score.ToString();
    }
}
