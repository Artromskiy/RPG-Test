using UnityEngine;
using UnityEngine.UI.Extensions;

public class PlayerSkillConnection : MonoBehaviour
{
    [SerializeField]
    private UILineRenderer _lineComponent;

    public void AddConnection(PlayerSkillView skill1, PlayerSkillView skill2)
    {
        _lineComponent.Points = new Vector2[2] { skill1.transform.position, skill2.transform.position };
    }
}
