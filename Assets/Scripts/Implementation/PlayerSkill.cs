using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerSkill : IEquatable<PlayerSkill>
{
    public readonly int id;
    public readonly int price;

    public bool Equals(PlayerSkill other)
    {
        return id == other.id;
    }

    public override bool Equals(object obj)
    {
        return obj is PlayerSkill skill && skill.Equals(this);
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}
