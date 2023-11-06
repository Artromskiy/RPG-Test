public interface IPlayerSkillsView : IView
{
    IGameEvent<PlayerSkill> OnSkillClicked { get; }
    IGameEvent OnObtainClicked { get; }
    IGameEvent OnForgetClicked { get; }
    IGameEvent OnForgetAllClicked { get; }
    bool CanObtainSelected { set; }
    bool CanForgetSelected { set; }

    /// <summary>
    /// This is workaround.
    /// As we can not create View that matches config
    /// we will provide config to presenter from view.
    /// Why we can't draw view based on data from presenter?
    /// We can but it's really hard and not perfect
    /// https://en.wikipedia.org/wiki/Graph_drawing.
    /// Best we can - validate matching of config and view
    /// </summary>
    public ISkillGraphConfig SkillGraphConfig { get; }
}
