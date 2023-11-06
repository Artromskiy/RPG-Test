using UnityEngine;
public struct SkillViewPair
{
    [SerializeField]
    private PlayerSkill _skill;
    [SerializeField]
    private PlayerSkillView _skillView;

    public readonly PlayerSkill Skill => _skill;
    public readonly PlayerSkillView View => _skillView;
}
