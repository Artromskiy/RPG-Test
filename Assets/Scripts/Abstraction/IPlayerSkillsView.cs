using System;

public interface IPlayerSkillsView : IView
{
    public event Action<PlayerSkill> OnSkillClicked;
    public event Action OnObtainClicked;
    public event Action OnForgetClicked;
    public event Action OnForgetAllClicked;
    public bool CanObtainSelected { set; }
    public bool CanForgetSelected { set; }
    //public PlayerSkill? SelectedSkill { set; }
}
