public class PlayerSkillsPresenter : Presenter<IPlayerSkillsView>, IPlayerSkillsPresenter
{
    private readonly ISkillGraphConfig _skillGraph;
    private readonly IPlayerSkillsModel _playerSkills;
    private readonly IPlayerScoreModel _playerScore;

    private readonly ReactiveField<int?> _selectedSkillId = new();
    private readonly ReactiveField<bool> _canForget = new();
    private readonly ReactiveField<bool> _canObtain = new();
    private readonly ReactiveField<int?> _price = new();

    public IReactiveField<int?> SelectedSkillId => _selectedSkillId;
    public IReactiveField<bool> CanForget => _canForget;
    public IReactiveField<bool> CanObtain => _canObtain;
    public IReactiveField<int?> Price => _price;

    private Graph<int, PlayerSkill> Graph => _skillGraph.SkillGraph;
    private PlayerSkill SelectedSkill => _selectedSkillId.Value == null ? null : Graph[_selectedSkillId.Value.Value].value;

    public PlayerSkillsPresenter(ISkillGraphConfig skillGraph, IPlayerSkillsModel playerSkills, IPlayerScoreModel playerScore, IPlayerSkillsView view) : base(view)
    {
        _skillGraph = skillGraph;
        _playerSkills = playerSkills;
        _playerScore = playerScore;

        _selectedSkillId.Event += ResolveSelection;

        View.SetConnections(Graph.GetConnections());

        View.OnForgetAllClicked.Event += ForgetAll;
        View.OnForgetClicked.Event += Forget;
        View.OnObtainClicked.Event += Obtain;
        View.OnSkillClicked.Event += _selectedSkillId.Invoke;

        _playerScore.OnScoreChanged.Event += OnScoreChanged;
        _playerSkills.OnSkillForgotten.Event += OnSkillForgotten;
        _playerSkills.OnSkillObtained.Event += OnSkillObtained;
        _playerSkills.OnAllForgotten.Event += OnAllForgotten;
    }

    protected override void Dispose(bool disposing)
    {
        View.OnForgetAllClicked.Event -= ForgetAll;
        View.OnForgetClicked.Event -= Forget;
        View.OnObtainClicked.Event -= Obtain;
        View.OnSkillClicked.Event -= _selectedSkillId.Invoke;

        _playerScore.OnScoreChanged.Event -= OnScoreChanged;
        _playerSkills.OnSkillForgotten.Event -= OnSkillForgotten;
        _playerSkills.OnSkillObtained.Event -= OnSkillObtained;
        _playerSkills.OnAllForgotten.Event -= OnAllForgotten;

        base.Dispose(disposing);
    }

    private void ForgetAll()
    {
        int addScore = 0;
        foreach (var skill in _playerSkills)
            addScore += skill.price;

        _playerScore.Score += addScore;
        _playerSkills.Clear();
    }

    private void OnScoreChanged(int _)
    {
        if (SkillNotSelected)
            return;
        UpdateReachability();
    }

    private void OnAllForgotten()
    {
        if (SkillNotSelected)
            return;
        UpdateReachability();
    }


    /// <summary>
    /// More performant way to react on skill being obtained comparing to ResolveSelection
    /// </summary>
    /// <param name="skill"></param>
    private void OnSkillObtained(PlayerSkill skill)
    {
        if (SkillNotSelected || !Graph.Contains(skill))
            return;
        if (SelectedSkill.Equals(skill))
        {
            _canObtain.Value = false;
            _canForget.Value = ReachableToForget();
            return;
        }
        UpdateReachability();
    }

    /// <summary>
    /// More performant way to react on skill being forgotten comparing to ResolveSelection
    /// </summary>
    /// <param name="skill"></param>
    private void OnSkillForgotten(PlayerSkill skill)
    {
        if (SkillNotSelected || !Graph.Contains(skill))
            return;
        if (SelectedSkill.Equals(skill))
        {
            _canObtain.Value = EnoughScore() && ReachableObtain();
            _canForget.Value = false;
            return;
        }
        UpdateReachability();
    }

    /// <summary>
    /// Provides basic validation on attempt of skill selection
    /// </summary>
    /// <param name="id"></param>
    private void ResolveSelection(int? id)
    {
        if (id == null)
        {
            _canForget.Value = false;
            _canObtain.Value = false;
            _price.Value = null;
            return;
        }
        else if (Graph.IsRoot(id.Value) || !Graph.Contains(id.Value))
        {
            _selectedSkillId.Value = null;
            return;
        }
        _price.Value = SelectedSkill.price;
        UpdateReachability();
    }
    /// <summary>
    /// Updates _canForget and _canObtain reactive fields based on current skill
    /// </summary>
    private void UpdateReachability()
    {
        if (IsNotObtained(SelectedSkill))
        {
            _canForget.Value = false;
            _canObtain.Value = EnoughScore() && ReachableObtain();
            return;
        }
        else
        {
            _canObtain.Value = false;
            _canForget.Value = ReachableToForget();
        }
    }

    private bool ReachableToForget()
    {
        foreach (var node in Graph[SelectedSkill.Key])
            if (_playerSkills.IsObtained(Graph[node].value) && !Graph.IsRootReachable(node, SelectedSkill)) // Execute from node to root as it's faster to check
                return false;
        return true;
    }
    private bool ReachableObtain() => Graph.IsReachableFromRoot(SelectedSkill, IsNotObtained); // Execute from root as it's main point
    private bool EnoughScore() => _playerScore.Score >= SelectedSkill.price;

    /// <summary>
    /// Just shortcut as using CurrentSkill involves many jumps with links, and (_selectedSkillId.Value == null) looks weird
    /// </summary>
    /// <returns></returns>
    private bool SkillNotSelected => _selectedSkillId.Value == null;

    private void Forget()
    {
        if (!_canForget.Value)
            return;
        _playerScore.Score += SelectedSkill.price;
        _playerSkills.Forget(SelectedSkill);
    }

    private void Obtain()
    {
        if (!_canObtain.Value)
            return;
        _playerScore.Score -= SelectedSkill.price;
        _playerSkills.Obtain(SelectedSkill);
    }

    private bool IsNotObtained(PlayerSkill skill)
    {
        return !_playerSkills.IsObtained(skill);
    }
}