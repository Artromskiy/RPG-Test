using TMPro;
using UnityEngine;

public class PlayerSkillView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _id;

    private readonly GameEvent<int> _onClicked = new();
    public IGameEvent<int> OnClicked => _onClicked;
}
