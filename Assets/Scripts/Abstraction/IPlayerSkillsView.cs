public interface IPlayerSkillsView : IView
{
    public IGameEvent<PlayerSkill> OnSkillClicked { get; }
    public IGameEvent OnObtainClicked { get; }
    public IGameEvent OnForgetClicked { get; }
    public IGameEvent OnForgetAllClicked { get; }
    public bool CanObtainSelected { set; }
    public bool CanForgetSelected { set; }

    /// <summary>
    /// This is workaround
    /// As we can not create View that match config
    /// we will provide config to presenter from view.
    /// Why we can't draw view based on data from presenter?
    /// We can but it's really hard and not perfect
    /// https://en.wikipedia.org/wiki/Graph_drawing
    /// Best we can - validate matching of config and view
    /// </summary>
    public ISkillGraphConfig SkillGraphConfig { get; }
}
