using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillsView : View, IPlayerSkillsView
{
    public bool CanObtainSelected { set; private get; }
    public bool CanForgetSelected { set; private get; }

    private readonly GameEvent<PlayerSkill> OnSkillClicked = new();
    private readonly GameEvent OnObtainClicked = new();
    private readonly GameEvent OnForgetClicked = new();
    private readonly GameEvent OnForgetAllClicked = new();


    IGameEvent<PlayerSkill> IPlayerSkillsView.OnSkillClicked => OnSkillClicked;
    IGameEvent IPlayerSkillsView.OnObtainClicked => OnObtainClicked;
    IGameEvent IPlayerSkillsView.OnForgetClicked => OnForgetClicked;
    IGameEvent IPlayerSkillsView.OnForgetAllClicked => OnForgetAllClicked;



    [SerializeField]
    private Button _obtainButton;
    [SerializeField]
    private Button _forgetButton;
    [SerializeField]
    private Button _forgetAllButton;

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
