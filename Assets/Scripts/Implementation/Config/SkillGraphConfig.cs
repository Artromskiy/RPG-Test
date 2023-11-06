using Newtonsoft.Json;

public class SkillGraphConfig : ISkillGraphConfig
{
    [JsonProperty]
    private readonly Graph<int, PlayerSkill> _skillGraph;
    public Graph<int, PlayerSkill> PlayerSkillGraph => _skillGraph;
}
