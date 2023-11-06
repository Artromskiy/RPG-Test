using Newtonsoft.Json;

public class SkillGraphConfig : ISkillGraphConfig
{
    [JsonProperty]
    private readonly SkillGraph<PlayerSkill> _skillGraph;
    public SkillGraph<PlayerSkill> PlayerSkillGraph => _skillGraph;
}
