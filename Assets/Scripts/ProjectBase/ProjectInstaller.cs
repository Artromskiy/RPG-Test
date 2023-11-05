using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddTransient(typeof(PlayerScoreView), typeof(IPlayerScoreView));
        descriptor.AddTransient(typeof(PlayerSkillsView), typeof(IPlayerSkillsView));

        descriptor.AddTransient(typeof(PlayerScoreModel), typeof(IPlayerScoreModel));
        descriptor.AddTransient(typeof(PlayerSkillsModel), typeof(IPlayerSkillsModel));

        descriptor.AddTransient(typeof(PlayerScorePresenter), typeof(IPlayerScorePresenter));
        descriptor.AddTransient(typeof(PlayerSkillsPresenter), typeof(IPlayerSkillsPresenter));

        var container = descriptor.Build();

        container.Construct<IPlayerSkillsPresenter>();
        container.Construct<IPlayerScorePresenter>();
    }
}
