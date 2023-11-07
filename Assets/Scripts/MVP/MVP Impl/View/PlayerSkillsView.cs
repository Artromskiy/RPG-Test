using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsView : View<IPlayerSkillsPresenter>, IPlayerSkillsView
{
    [SerializeField]
    private Button _obtainButton;
    [SerializeField]
    private Button _forgetButton;
    [SerializeField]
    private Button _forgetAllButton;
    [SerializeField]
    private RectTransform _connectionsHolder;
    [SerializeField]
    private RectTransform _priceHolder;
    [SerializeField]
    private TextMeshProUGUI _priceText;
    [SerializeField]
    private Button _backgroundButton;

    /// <summary>
    /// This is workaround.
    /// As we can not create View that matches given config
    /// we will use just indices of elements as key (Note that PlayerSkill: Keyd<int>).
    /// Why we can't draw view based on data from presenter?
    /// We can but it's really hard and not perfect
    /// https://en.wikipedia.org/wiki/Graph_drawing.
    /// Best we can - validate matching of config and view
    /// </summary>
    [SerializeField]
    private List<NodeView> _playerSkillViews;

    private NodeView _selectedNode;

    private readonly GameEvent<int?> _onSkillClicked = new();
    private readonly GameEvent _onObtainClicked = new();
    private readonly GameEvent _onForgetClicked = new();
    private readonly GameEvent _onForgetAllClicked = new();

    IGameEvent<int?> IPlayerSkillsView.OnSkillClicked => _onSkillClicked;
    IGameEvent IPlayerSkillsView.OnObtainClicked => _onObtainClicked;
    IGameEvent IPlayerSkillsView.OnForgetClicked => _onForgetClicked;
    IGameEvent IPlayerSkillsView.OnForgetAllClicked => _onForgetAllClicked;

    protected override void Init()
    {
        for (int i = 0; i < _playerSkillViews.Count; i++)
        {
            _playerSkillViews[i].SetId(i);
            _playerSkillViews[i].OnClick += _onSkillClicked.Invoke;
        }

        _backgroundButton.onClick.AddListener(OnBackgroundClicked);
        _obtainButton.onClick.AddListener(_onObtainClicked.Invoke);
        _forgetButton.onClick.AddListener(_onForgetClicked.Invoke);
        _forgetAllButton.onClick.AddListener(_onForgetAllClicked.Invoke);

        Presenter.CanForget.Event += _forgetButton.gameObject.SetActive;
        Presenter.CanObtain.Event += _obtainButton.gameObject.SetActive;
        Presenter.SelectedSkillId.Event += HighlightSkill;
        Presenter.Price.Event += UpdatePrice;

        _forgetButton.gameObject.SetActive(Presenter.CanForget.Value);
        _obtainButton.gameObject.SetActive(Presenter.CanObtain.Value);
        HighlightSkill(Presenter.SelectedSkillId.Value);
        UpdatePrice(Presenter.Price.Value);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _backgroundButton.onClick.RemoveListener(OnBackgroundClicked);
            _obtainButton.onClick.RemoveListener(_onObtainClicked.Invoke);
            _forgetButton.onClick.RemoveListener(_onForgetClicked.Invoke);
            _forgetAllButton.onClick.RemoveListener(_onForgetAllClicked.Invoke);

            Presenter.CanForget.Event -= _forgetButton.gameObject.SetActive;
            Presenter.CanObtain.Event -= _obtainButton.gameObject.SetActive;
            Presenter.SelectedSkillId.Event -= HighlightSkill;
            Presenter.Price.Event -= UpdatePrice;
        }

        base.Dispose(disposing);
    }

    private void OnBackgroundClicked()
    {
        _onSkillClicked?.Invoke(null);
    }

    private void UpdatePrice(int? price)
    {
        if (price == null)
            _priceHolder.gameObject.SetActive(false);
        else
        {
            _priceHolder.gameObject.SetActive(true);
            _priceText.text = price.Value.ToString();
        }
    }

    private void HighlightSkill(int? skillId)
    {
        if (_selectedNode != null)
            _selectedNode.Highlight(false);
        if (skillId == null || _playerSkillViews.Count < skillId.Value)
            _selectedNode = null;
        else
            (_selectedNode = _playerSkillViews[skillId.Value]).Highlight(true);
    }

    public void SetConnections(ReadOnlySpan<(int element1, int element2)> conncections)
    {
        foreach (var (element1, element2) in conncections)
            if (element1 < _playerSkillViews.Count && element2 < _playerSkillViews.Count)
                _playerSkillViews[element1].Connect(_playerSkillViews[element2], _connectionsHolder);
    }
}