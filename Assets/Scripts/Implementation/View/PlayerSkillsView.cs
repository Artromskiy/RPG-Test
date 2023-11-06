using Reflex.Attributes;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsView : View, IPlayerSkillsView
{
    [SerializeField]
    private Button _obtainButton;
    [SerializeField]
    private Button _forgetButton;
    [SerializeField]
    private Button _forgetAllButton;

    [SerializeField]
    private List<SkillViewPair> _playerSkillViews;                              // SkillViewPair created to avoid mixing of data and view
    private readonly Dictionary<int, PlayerSkillView> _skillToView = new();     // The best way is to use Odin serializer/editor instead of such workarounds

    public bool CanObtainSelected { get; set; }
    public bool CanForgetSelected { get; set; }

    private readonly GameEvent<PlayerSkill> OnSkillClicked = new();
    private readonly GameEvent OnObtainClicked = new();
    private readonly GameEvent OnForgetClicked = new();
    private readonly GameEvent OnForgetAllClicked = new();

    IGameEvent<PlayerSkill> IPlayerSkillsView.OnSkillClicked => OnSkillClicked;
    IGameEvent IPlayerSkillsView.OnObtainClicked => OnObtainClicked;
    IGameEvent IPlayerSkillsView.OnForgetClicked => OnForgetClicked;
    IGameEvent IPlayerSkillsView.OnForgetAllClicked => OnForgetAllClicked;
    public ISkillGraphConfig SkillGraphConfig { get; private set; }


    private void Awake()
    {
        foreach (var item in _playerSkillViews)
            _skillToView.Add(item.Id, item.View);
    }

    [Inject]
    private void Init(ISkillGraphConfig config)
    {
        SkillGraphConfig = config;
        CreateConnections();
    }

    private void CreateConnections()
    {
        var graph = SkillGraphConfig.PlayerSkillGraph;
        foreach (var item in graph)
            if (_skillToView.TryGetValue(item.value.Key, out var skillView))
                foreach (var connection in item)
                    if (_skillToView.TryGetValue(connection, out var skillViewToConnect))
                        skillView.Connect(skillViewToConnect);
    }

    private void OnEnable()
    {
        _obtainButton.onClick.AddListener(OnObtainClicked.Invoke);
        _forgetButton.onClick.AddListener(OnForgetClicked.Invoke);
        _forgetAllButton.onClick.AddListener(OnForgetAllClicked.Invoke);
    }

    private void OnDisable()
    {
        _obtainButton.onClick.RemoveListener(OnObtainClicked.Invoke);
        _forgetButton.onClick.RemoveListener(OnForgetClicked.Invoke);
        _forgetAllButton.onClick.RemoveListener(OnForgetAllClicked.Invoke);
    }
}