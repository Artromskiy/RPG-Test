using System.Collections.Generic;

public interface IPlayerSkillsModel : IEnumerable<PlayerSkill>, IModel<IPlayerSkillsModel>
{
    public HashSet<PlayerSkill> Skills { get; }
    public bool IsObtained(PlayerSkill skil);
    public void Obtain(PlayerSkill skill);
    public void Forget(PlayerSkill skill);
    public IGameEvent<PlayerSkill> OnSkillObtained { get; }
    public IGameEvent<PlayerSkill> OnSkillForgotten { get; }
    public IGameEvent OnAllForgotten { get; }
    public void Clear();
}