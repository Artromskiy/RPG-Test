public interface ISkillGraphConfig : IConfig
{
    Graph<int, PlayerSkill> SkillGraph { get; }
}
