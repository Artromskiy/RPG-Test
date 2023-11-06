public interface ISkillGraphConfig:IConfig
{
    Graph<int, PlayerSkill> PlayerSkillGraph { get; }
}
