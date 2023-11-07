using System;
using UnityEngine;

[Serializable]
public struct SkillViewPair
{
    [SerializeField]
    private int id;
    [SerializeField]
    private NodeView _skillView;

    public readonly int Id => id;
    public readonly NodeView View => _skillView;
}
