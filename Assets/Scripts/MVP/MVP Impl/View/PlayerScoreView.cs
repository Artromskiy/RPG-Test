using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreView : View<IPlayerScorePresenter>, IPlayerScoreView
{
    [SerializeField]
    private Button _earnScoreButton;
    [SerializeField]
    private Button _loseAllButton;
    [SerializeField]
    private TextMeshProUGUI _scoreTextCounter;

    private readonly GameEvent _onRequestEarn = new();
    private readonly GameEvent _onRequestLoseAll = new();
    IGameEvent IPlayerScoreView.OnRequestEarn => _onRequestEarn;
    IGameEvent IPlayerScoreView.OnRequestLoseAll => _onRequestLoseAll;

    protected override void Init()
    {
        _earnScoreButton.onClick.AddListener(_onRequestEarn.Invoke);
        _loseAllButton.onClick.AddListener(_onRequestLoseAll.Invoke);

        Presenter.Score.Event += UpdateScore;

        UpdateScore(Presenter.Score.Value);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _earnScoreButton.onClick.RemoveListener(_onRequestEarn.Invoke);
            _loseAllButton.onClick.RemoveListener(_onRequestLoseAll.Invoke);

            Presenter.Score.Event -= UpdateScore;
        }

        base.Dispose(disposing);
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
