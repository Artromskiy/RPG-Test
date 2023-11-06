using System.Collections.Generic;

public class PlayerSkillsModel : Model<IPlayerSkillsModel>, IPlayerSkillsModel
{
    public static string ModelKey { get; } = "PlayerSkillsModel";

    private readonly HashSet<PlayerSkill> _playerSkills;

    public HashSet<PlayerSkill> Skills => new(_playerSkills);
    public bool IsObtained(PlayerSkill skil) => _playerSkills.Contains(skil);

    public void Obtain(PlayerSkill skill)
    {
        _playerSkills.Add(skill);
        InvokeModelChange();
    }
    public void Forget(PlayerSkill skill)
    {
        _playerSkills.Remove(skill);
        InvokeModelChange();
    }
    public void Clear()
    {
        _playerSkills.Clear();
        InvokeModelChange();
    }
}
