using Reflex.Core;
using UnityEngine;

public class ProjectInstaller : MonoBehaviour, IInstaller
{
    public void InstallBindings(ContainerDescriptor descriptor)
    {
        descriptor.AddInstance(ModelCreator<PlayerScoreModel>.Create("lol"), typeof(IPlayerScoreModel));
        //descriptor.AddInstance(PlayerSkillsModel.Create(PlayerSkillsModel.ModelKey), typeof(IPlayerSkillsModel));

        descriptor.AddTransient(typeof(PlayerScorePresenter), typeof(IPlayerScorePresenter));
        //descriptor.AddTransient(typeof(PlayerSkillsPresenter), typeof(IPlayerSkillsPresenter));
    }
}
