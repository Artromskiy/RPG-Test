using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreView : View, IPlayerScoreView
{
    [SerializeField]
    private Button _earnScoreButton;
    [SerializeField]
    private TextMeshProUGUI _scoreTextCounter;

    public GameEvent OnRequestEarn = new();
    IGameEvent IPlayerScoreView.OnRequestEarn => OnRequestEarn;

    public int Score
    {
        set => UpdateScore(value);
    }

    private void OnEnable()
    {
        _earnScoreButton.onClick.AddListener(OnRequestEarn.Invoke);
    }

    private void OnDisable()
    {
        _earnScoreButton.onClick.RemoveListener(OnRequestEarn.Invoke);
    }

    private void UpdateScore(int score)
    {
        _scoreTextCounter.text = score.ToString();
    }
}
