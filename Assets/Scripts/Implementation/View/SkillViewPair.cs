using System;
using UnityEngine;

[Serializable]
public struct SkillViewPair
{
    [SerializeField]
    private int id;
    [SerializeField]
    private PlayerSkillView _skillView;

    public readonly int Id => id;
    public readonly PlayerSkillView View => _skillView;
}
