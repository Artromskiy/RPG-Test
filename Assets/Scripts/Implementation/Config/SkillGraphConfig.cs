public class SkillGraphConfig : Config<SkillGraph<PlayerSkill>>, ISkillGraphConfig
{
    protected override string ConfigKey { get; } = "SkillGraphConfig";
}
