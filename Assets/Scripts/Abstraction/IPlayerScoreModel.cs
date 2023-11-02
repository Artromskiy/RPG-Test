using System;

public interface IPlayerScoreModel : IModel
{
    public event Action<int> OnScoreChanged;
    public int Score { get; set; }
}
