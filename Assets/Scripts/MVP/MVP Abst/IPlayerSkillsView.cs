using System;

public interface IPlayerSkillsView : IView
{
    IGameEvent<int?> OnSkillClicked { get; }
    IGameEvent OnObtainClicked { get; }
    IGameEvent OnForgetClicked { get; }
    IGameEvent OnForgetAllClicked { get; }
    void SetConnections(ReadOnlySpan<(int element1, int element2)> connections);
}
