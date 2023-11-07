using UnityEngine;
using UnityEngine.UI.Extensions;

public class NodeConnection : MonoBehaviour
{
    [SerializeField]
    private UILineConnector _lineConnector;

    public void AddConnection(NodeView skill1, NodeView skill2)
    {
        _lineConnector.transforms = new RectTransform[] { skill1.RectTransform, skill2.RectTransform}; 
    }
}
