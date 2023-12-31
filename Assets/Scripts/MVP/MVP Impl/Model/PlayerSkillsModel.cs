using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkillsModel : Model<IPlayerSkillsModel>, IPlayerSkillsModel
{
    [JsonProperty]
    private readonly HashSet<PlayerSkill> _playerSkills = new();

    private readonly GameEvent<PlayerSkill> _onSkillObtained = new();
    private readonly GameEvent<PlayerSkill> _onSkillForgoten = new();
    private readonly GameEvent _onAllForgoten = new();

    public HashSet<PlayerSkill> Skills => new(_playerSkills);
    public IGameEvent<PlayerSkill> OnSkillObtained => _onSkillObtained;
    public IGameEvent<PlayerSkill> OnSkillForgotten => _onSkillForgoten;
    public IGameEvent OnAllForgotten => _onAllForgoten;
    public bool IsObtained(PlayerSkill skil) => _playerSkills.Contains(skil);

    public void Obtain(PlayerSkill skill)
    {
        if (_playerSkills.Add(skill))
        {
            _onSkillObtained.Invoke(skill);
            InvokeModelChange();
        }
    }
    public void Forget(PlayerSkill skill)
    {
        if (_playerSkills.Remove(skill))
        {
            _onSkillForgoten.Invoke(skill);
            InvokeModelChange();
        }
    }

    public void Clear()
    {
        _playerSkills.Clear();
        _onAllForgoten?.Invoke();
        InvokeModelChange();
    }

    public IEnumerator<PlayerSkill> GetEnumerator() => _playerSkills.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
