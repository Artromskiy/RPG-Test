using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreView : View<IPlayerScorePresenter>, IPlayerScoreView
{
    [SerializeField]
    private Button _earnScoreButton;
    [SerializeField]
    private TextMeshProUGUI _scoreTextCounter;

    public event Action OnRequestEarn;

    private void Start()
    {
        Presenter.OnScoreChanged += UpdateScore;
        UpdateScore(Presenter.Score);
        _earnScoreButton.onClick.AddListener(OnRequestEarn.Invoke);
    }

    private void UpdateScore(int score)
    {
        _scoreTextCounter.text = score.ToString();
    }
}
