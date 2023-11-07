public interface IPlayerSkillsPresenter : IPresenter<IPlayerSkillsView>
{
    public IReactiveField<int?> SelectedSkillId { get; }
    public IReactiveField<bool> CanForget { get; }
    public IReactiveField<bool> CanObtain { get; }
    public IReactiveField<int?> Price { get; }
}
