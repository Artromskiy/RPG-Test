using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NodeView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _textId;
    [SerializeField]
    private NodeConnection _linePrefab;
    [SerializeField]
    private Button _skillButton;
    [SerializeField]
    private RectTransform _highlight;
    public RectTransform RectTransform => transform as RectTransform;
    private Dictionary<NodeView, NodeConnection> _connected;

    private int _id;

    public event Action<int?> OnClick;

    private void Start()
    {
        _skillButton.onClick.AddListener(Click);
    }

    public void Highlight(bool value) => _highlight.gameObject.SetActive(value);
    private void Click() => OnClick?.Invoke(_id);
    public void SetId(int value)
    {
        _id = value;
        _textId.text = _id.ToString();
    }
    public void Connect(NodeView connect, RectTransform parent)
    {
        if (IsConnected(connect) || connect.IsConnected(this))
            return;

        var line = Instantiate(_linePrefab, parent, false);
        line.AddConnection(this, connect);
        (_connected ??= new()).Add(connect, line);
    }

    private bool IsConnected(NodeView view)
    {
        return _connected != null && _connected.ContainsKey(view);
    }
}
