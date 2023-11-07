public interface IPlayerScorePresenter : IPresenter<IPlayerScoreView>
{
    public IReactiveField<int> Score { get; }
}
