using System;
using UnityEngine;

[Serializable]
public class PlayerSkill : IEquatable<PlayerSkill>
{
    [SerializeField]
    private int _id;
    [SerializeField]
    private int _price;

    public int Id => _id;
    public int Price => _price;

    public bool Equals(PlayerSkill other)
    {
        return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
        return obj is PlayerSkill skill && skill.Equals(this);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
