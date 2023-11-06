using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddInstance(ConfigCreator<SkillGraphConfig>.Create(FileConstants.SkillGraphConfig), typeof(ISkillGraphConfig));
        descriptor.AddInstance(ModelCreator<PlayerScoreModel>.Create(FileConstants.PlayerScore), typeof(IPlayerScoreModel));
        descriptor.AddInstance(ModelCreator<PlayerSkillsModel>.Create(FileConstants.PlayerSkills), typeof(IPlayerSkillsModel));

        descriptor.AddTransient(typeof(PlayerScorePresenter), typeof(IPlayerScorePresenter));
        descriptor.AddTransient(typeof(PlayerSkillsPresenter), typeof(IPlayerSkillsPresenter));
    }
}
