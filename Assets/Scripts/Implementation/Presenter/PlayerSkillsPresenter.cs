using System.Diagnostics;

public class PlayerSkillsPresenter : Presenter<IPlayerSkillsView>, IPlayerSkillsPresenter
{
    private readonly IPlayerSkillsModel _playerSkills;
    private readonly IPlayerScoreModel _playerScore;
    public SkillGraph<PlayerSkill> SkillGraph => View.SkillGraphConfig.PlayerSkillGraph;

    private PlayerSkill _selectedSkill;

    public PlayerSkillsPresenter(IPlayerSkillsModel playerSkills, IPlayerScoreModel playerScore, IPlayerSkillsView view):base(view)
    {
        _playerSkills = playerSkills;
        _playerScore = playerScore;

        _playerSkills.OnModelChanged += OnSkillsChanged;
        _playerScore.OnModelChanged += OnScoreChanged;

        View.OnForgetAllClicked.Event += ForgetAll;
        View.OnForgetClicked.Event += Forget;
        View.OnObtainClicked.Event += Obtain;
        View.OnSkillClicked.Event += Select;
    }

    protected override void Dispose(bool disposing)
    {
        _playerSkills.OnModelChanged -= OnSkillsChanged;
        _playerScore.OnModelChanged -= OnScoreChanged;

        View.OnForgetAllClicked.Event -= ForgetAll;
        View.OnForgetClicked.Event -= Forget;
        View.OnObtainClicked.Event -= Obtain;
        View.OnSkillClicked.Event -= Select;

        base.Dispose(disposing);
    }

    private void ForgetAll()
    {
        _playerSkills.Clear();
    }

    private void Select(PlayerSkill skill)
    {
        _selectedSkill = skill;
        UpdateViewInfo();
    }

    private void Forget()
    {
        Debug.Assert(CanForget());

        _playerScore.Score += _selectedSkill.price;
        _playerSkills.Forget(_selectedSkill);
    }

    private void Obtain()
    {
        Debug.Assert(CanObtain());

        _playerScore.Score -= _selectedSkill.price;
        _playerSkills.Obtain(_selectedSkill);
    }

    private void OnScoreChanged(IPlayerScoreModel score)
    {
        UpdateViewInfo();
    }

    private void OnSkillsChanged(IPlayerSkillsModel skills)
    {
        UpdateViewInfo();
    }

    private void UpdateViewInfo()
    {
        if (_selectedSkill != null)
        {
            View.CanForgetSelected = false;
            View.CanObtainSelected = false;
            return;
        }
        PlayerSkill selectedSkill = _selectedSkill;
        if (_playerSkills.IsObtained(selectedSkill))
        {
            var obtainedSkillsNearSelected = _playerSkills.Skills;
            obtainedSkillsNearSelected.IntersectWith(SkillGraph.GetNear(_selectedSkill));

            View.CanForgetSelected = SkillGraph.IsAllReachable(obtainedSkillsNearSelected, selectedSkill);
            View.CanObtainSelected = false;
        }
        else
        {
            View.CanObtainSelected = _playerScore.Score >= selectedSkill.price && SkillGraph.IsReachableFromRoot(selectedSkill, _playerSkills.IsObtained);
            View.CanForgetSelected = false;
        }
    }

    private bool CanObtain()
    {
        return _selectedSkill != null && !_playerSkills.IsObtained(_selectedSkill) && SkillGraph.IsReachableFromRoot(_selectedSkill, _playerSkills.IsObtained);
    }

    private bool CanForget()
    {
        if (_selectedSkill != null)
            return false;

        var obtainedSkillsNearSelected = _playerSkills.Skills;
        obtainedSkillsNearSelected.IntersectWith(SkillGraph.GetNear(_selectedSkill));

        return _selectedSkill != null && _playerSkills.IsObtained(_selectedSkill) && SkillGraph.IsAllReachable(obtainedSkillsNearSelected, _selectedSkill);
    }
}