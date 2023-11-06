using System.Collections.Generic;

public interface IPlayerSkillsModel : IModel<IPlayerSkillsModel>
{
    public HashSet<PlayerSkill> Skills { get; }
    public bool IsObtained(PlayerSkill skil);
    public void Obtain(PlayerSkill skill);
    public void Forget(PlayerSkill skill);
    public void Clear();
}