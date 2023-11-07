public interface IPlayerScoreView : IView
{
    public IGameEvent OnRequestEarn { get; }
    public IGameEvent OnRequestLoseAll { get; }
}