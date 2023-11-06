using System;

public interface IPlayerSkillsView : IView
{
    public IGameEvent<PlayerSkill> OnSkillClicked { get; }
    public IGameEvent OnObtainClicked{get;}
    public IGameEvent OnForgetClicked{get;}
    public IGameEvent OnForgetAllClicked { get; }
    public bool CanObtainSelected { set; }
    public bool CanForgetSelected { set; }
    //public PlayerSkill? SelectedSkill { set; }
}
