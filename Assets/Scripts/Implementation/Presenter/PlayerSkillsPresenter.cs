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

        _selectedSkillId.Value = null;
    }

    protected override void Dispose(bool disposing)
    {
        View.OnForgetAllClicked.Event -= ForgetAll;
        View.OnForgetClicked.Event -= Forget;
        View.OnObtainClicked.Event -= Obtain;
        View.OnSkillClicked.Event -= _selectedSkillId.Invoke;

        base.Dispose(disposing);
    }

    private void ForgetAll()
    {
        foreach (var skill in _playerSkills)
            _playerScore.Score += skill.price;
        _playerSkills.Clear();
    }

    private void OnScoreChanged(int _)
    {
        if (SelectedSkill == null)
            return;
        if (IsNotObtained(SelectedSkill) && EnoughScore())
            _canObtain.Value = EnoughScore() && ReachableObtain();
    }

    private void OnSkillObtained(PlayerSkill skill)
    {
        if (SelectedSkill == null || !Graph.Contains(skill))
            return;
        if(SelectedSkill == skill)
        {
            _canObtain.Value = false;
            _canForget.Value = ReachableToForget();
            return;
        }
        UpdateReachability();
    }

    private void OnSkillForgotten(PlayerSkill skill)
    {
        if (SelectedSkill == null || !Graph.Contains(skill))
            return;
        if(SelectedSkill == skill)
        {
            _canObtain.Value = EnoughScore() && ReachableObtain();
            _canForget.Value = false;
            return;
        }
        UpdateReachability();
    }

    private void ResolveSelection(int? id)
    {
        if (id != null && !_skillGraph.SkillGraph.Contains(id.Value))
        {
            _selectedSkillId.Value = null;
            return;
        }
        if (id == null)
        {
            _canForget.Value = false;
            _canObtain.Value = false;
            _price.Value = null;
            return;
        }
        _price.Value = SelectedSkill.price;
        UpdateReachability();
        
    }

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
        foreach (var item in Graph[SelectedSkill.Key])
            if (_playerSkills.IsObtained(Graph[item].value) && !Graph.IsRootReachable(item, SelectedSkill))
                return false;
        return true;
    }
    private bool ReachableObtain() => Graph.IsReachableFromRoot(SelectedSkill, IsNotObtained);
    private bool EnoughScore()=> _playerScore.Score >= SelectedSkill.price;

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