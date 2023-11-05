using Reflex.Attributes;
using System;
using System.Diagnostics;

public class PlayerSkillsPresenter : Presenter<IPlayerSkillsView, IPlayerSkillsPresenter>, IPlayerSkillsPresenter, IDisposable
{
    [Inject]
    private readonly IPlayerSkillsModel _playerSkills;
    [Inject]
    private readonly IPlayerScoreModel _playerScore;
    public SkillGraph<PlayerSkill> SkillGraph { get; }

    private PlayerSkill? _selectedSkill;

    public PlayerSkillsPresenter()
    {
        _playerSkills.OnModelChanged += OnSkillsChanged;
        _playerScore.OnModelChanged += OnScoreChanged;

        View.OnForgetAllClicked += ForgetAll;
        View.OnForgetClicked += Forget;
        View.OnObtainClicked += Obtain;
        View.OnSkillClicked += Select;
    }

    protected override void Dispose(bool disposing)
    {
        _playerSkills.OnModelChanged -= OnSkillsChanged;
        _playerScore.OnModelChanged -= OnScoreChanged;

        View.OnForgetAllClicked -= ForgetAll;
        View.OnForgetClicked -= Forget;
        View.OnObtainClicked -= Obtain;
        View.OnSkillClicked -= Select;

        base.Dispose();
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

        _playerScore.Score += _selectedSkill.Value.price;
        _playerSkills.Forget(_selectedSkill.Value);
    }

    private void Obtain()
    {
        Debug.Assert(CanObtain());

        _playerScore.Score -= _selectedSkill.Value.price;
        _playerSkills.Obtain(_selectedSkill.Value);
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
        if (!_selectedSkill.HasValue)
        {
            View.CanForgetSelected = false;
            View.CanObtainSelected = false;
            return;
        }
        PlayerSkill selectedSkill = _selectedSkill.Value;
        if (_playerSkills.IsObtained(selectedSkill))
        {
            var obtainedSkillsNearSelected = _playerSkills.Skills;
            obtainedSkillsNearSelected.IntersectWith(SkillGraph.GetNear(_selectedSkill.Value));

            View.CanForgetSelected = SkillGraph.IsAllReachable(obtainedSkillsNearSelected, selectedSkill);
            View.CanObtainSelected = false;
        }
        else
        {
            View.CanObtainSelected = _playerScore.Score >= selectedSkill.price && SkillGraph.IsReachable(selectedSkill, _playerSkills.IsObtained);
            View.CanForgetSelected = false;
        }
    }

    private bool CanObtain()
    {
        return _selectedSkill.HasValue && !_playerSkills.IsObtained(_selectedSkill.Value) && SkillGraph.IsReachable(_selectedSkill.Value, _playerSkills.IsObtained);
    }

    private bool CanForget()
    {
        if (!_selectedSkill.HasValue)
            return false;

        var obtainedSkillsNearSelected = _playerSkills.Skills;
        obtainedSkillsNearSelected.IntersectWith(SkillGraph.GetNear(_selectedSkill.Value));

        return _selectedSkill.HasValue && _playerSkills.IsObtained(_selectedSkill.Value) && SkillGraph.IsAllReachable(obtainedSkillsNearSelected, _selectedSkill.Value);
    }
}