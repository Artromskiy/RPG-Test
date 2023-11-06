using Reflex.Core;
using TNRD;
using UnityEngine;

public class SceneInstaller : MonoBehaviour, IInstaller
{
    [SerializeField]
    private SerializableInterface<IPlayerScoreView> _playerScoreViewInstance;
    [SerializeField]
    private SerializableInterface<IPlayerSkillsView> _playerSkillsViewInstance;

    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddInstance(_playerScoreViewInstance.Value, _playerScoreViewInstance.InterfaceType);
        //descriptor.AddInstance(_playerSkillsViewInstance.Value, _playerSkillsViewInstance.InterfaceType);
        descriptor.OnContainerBuilt += (c) =>
        {
            c.Resolve<IPlayerScorePresenter>();
        };
    }
}
