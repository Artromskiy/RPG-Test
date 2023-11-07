using System;

public interface IPlayerScoreModel : IModel<IPlayerScoreModel>
{
    public int Score { get; set; }
    public IGameEvent<int> OnScoreChanged { get; }
}
