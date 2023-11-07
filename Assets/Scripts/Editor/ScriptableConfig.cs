using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Workaround to create and save config inside editor
/// </summary>
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
    private List<NodeConnection> _connections;

    [Serializable]
    private struct NodeConnection
    {
        public int from;
        public int to;
    }

    [ContextMenu("Save")]
    private void Save()
    {
        Dictionary<PlayerSkill, HashSet<PlayerSkill>> skills = new();
        for (int i = 0; i < skillPrices.Count; i++)
        {
            var skill = new PlayerSkill(i, skillPrices[i]);
            skills.Add(skill, new());
        }
        for (int i = 0; i < _connections.Count; i++)
        {
            var fromIndex = _connections[i].from;
            var toIndex = _connections[i].to;
            var from = new PlayerSkill(fromIndex, skillPrices[fromIndex]);
            var to = new PlayerSkill(toIndex, skillPrices[toIndex]);
            skills[from].Add(to);
            skills[to].Add(from);
        }
        Graph<int, PlayerSkill> playerSkills = new(0, skills);
        var serializedData = JsonConvert.SerializeObject(new SkillGraphConfig(playerSkills));
        File.WriteAllText($"{Application.dataPath}/Resources/{Key}.json", serializedData, Encoding.UTF8);
    }
}
