using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/ScriptableConfig")]
public class ScriptableConfig : ScriptableObject
{
    [SerializeField]
    private string Key;

    [SerializeField]
    private PlayerSkill _root;
    [SerializeField]
    private List<int> skillPrices;
    [SerializeField]
    private List<PlayerSkillConnection> _connections;

    [Serializable]
    private struct PlayerSkillConnection
    {
        [SerializeField]
        private int from;
        [SerializeField]
        private int to;
    }

    private void Save()
    {

    }
}
