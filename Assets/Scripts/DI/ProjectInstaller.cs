using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddTransient(typeof(GameEvent));
        descriptor.AddTransient(typeof(GameEvent<>));

        descriptor.AddSingleton(typeof(PlayerScoreModel), typeof(IPlayerScoreModel));
        descriptor.AddSingleton(typeof(PlayerSkillsModel), typeof(IPlayerSkillsModel));

        descriptor.AddTransient(typeof(PlayerScorePresenter), typeof(IPlayerScorePresenter));
        descriptor.AddTransient(typeof(PlayerSkillsPresenter), typeof(IPlayerSkillsPresenter));
    }
}
