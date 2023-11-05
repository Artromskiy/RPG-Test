using System;
using UnityEngine.UI;

public class PlayerSkillsView : View, IPlayerSkillsView
{
    public bool CanObtainSelected { set; private get; }
    public bool CanForgetSelected { set; private get; }
    //public PlayerSkill? SelectedSkill { set; private get; }

    public event Action<PlayerSkill> OnSkillClicked;
    public event Action OnObtainClicked;
    public event Action OnForgetClicked;

    public event Action OnForgetAllClicked;

    private readonly Button _obtainButton;
    private readonly Button _forgetButton;
    private readonly Button _forgetAllButton;

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
