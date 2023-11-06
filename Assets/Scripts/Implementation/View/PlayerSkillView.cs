using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSkillView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _id;
    [SerializeField]
    private PlayerSkillConnection _linePrefab;
    private readonly GameEvent<int> _onClicked = new();
    public IGameEvent<int> OnClicked => _onClicked;

    private HashSet<PlayerSkillView> _connected;

    public void Connect(PlayerSkillView connect)
    {
        if (!connect.IsConnected(connect) && !IsConnected(connect))
        {
            var line = Instantiate(_linePrefab, transform.parent.transform);
            line.AddConnection(this, connect);
            (_connected ??= new()).Add(connect);
        }
    }

    private bool IsConnected(PlayerSkillView view)
    {
        return _connected == null || !_connected.Contains(view);
    }
}
