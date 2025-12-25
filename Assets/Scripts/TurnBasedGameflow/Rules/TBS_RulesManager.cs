using System;
using System.Collections.Generic;
using UnityEngine;

public class TBS_RulesManager : MonoBehaviour
{
    // Менеджер правил пошаговой системы
    // Такая прикольная штука
    // Отвечает за правила следующих типов:
    // - правила перед началом хода
    // - правила учёта очков
    // - правила возможности действий
    // - правила победы или поражения
    // - правила после конца хода
    // - правила после каждого круга игроков
    // - правила перед началом игры

    public static TBS_RulesManager Instance { get; private set; }
    private GlobalFlags _globalFlags;

    // Правила
    private List<IRule> _rules;
    private List<IRule> Rules {
        get => _rules;
        set 
        {
            _rules = value;
            OnRulesChanged?.Invoke();
        }
    }

    // Кешируем значения правил по категориям
    public List<IRule> RulesBeforeTurnCashed { get; private set; } = new();
    public List<IRule> RulesAfterTurnCashed { get; private set; } = new();
    public List<IRule> RulesToCalculatePointsCashed { get; private set; } = new();
    public List<IRule> RulesToAllowActionCashed { get; private set; } = new();
    public List<IRule> RulesToWinOrDefeatCashed { get; private set; } = new();
    public List<IRule> RulesBeforeGameStartCashed { get; private set; } = new();
    public List<IRule> RulesAfterCycleEndedCashed { get; private set; } = new();

    private event Action OnRulesChanged;
    public event Action OnCashedRulesRecalculated;

    // Config
    private TBS_RulesConfigSO _rulesConfigSO;

    private void Awake()
    {
        Instance = this;
        OnRulesChanged += HandleRulesChanged;
    }

    private void OnDestroy()
    {
        OnRulesChanged -= HandleRulesChanged;
    }

    public void Init(GlobalFlags globalFlags, TBS_InitConfigSO config)
    {
        _globalFlags = globalFlags;
        _rulesConfigSO = config.rulesConfig;

        Load(config.currentRulesConfig);
    }

    private void Load(TBS_CurrentRulesConfigSO currentRules)
    {
        // Загружаем правила
        _rules = new List<IRule>();
        foreach (var rule in currentRules.rules)
        {
            string ruleId = rule.linkedRuleID;
            if (_rulesConfigSO.HasRule(ruleId)) _rules.Add(_rulesConfigSO.GetRule(ruleId).ruleInstance);
        }

        RecalculateCashedRules();
    }

    private void RecalculateCashedRules()
    {
        // Кешируем правила когда они поменялись
        // Как на счёт оптимизировать и не удалять всё, а просто добавлять и удалять по необходимости

        RulesBeforeTurnCashed.Clear();
        RulesAfterTurnCashed.Clear();
        RulesToCalculatePointsCashed.Clear();
        RulesToAllowActionCashed.Clear();
        RulesToWinOrDefeatCashed.Clear();
        RulesBeforeGameStartCashed.Clear();

        foreach (var rule in Rules)
        {
            switch (rule.ruleType)
            {
                case RuleType.BeforeTurn:
                    RulesBeforeTurnCashed.Add(rule);
                    break;
                case RuleType.AfterTurn:
                    RulesAfterTurnCashed.Add(rule);
                    break;
                case RuleType.ToCalculatePoints:
                    RulesToCalculatePointsCashed.Add(rule);
                    break;
                case RuleType.ToAllowAction:
                    RulesToAllowActionCashed.Add(rule);
                    break;
                case RuleType.ToWinOrDefeat:
                    RulesToWinOrDefeatCashed.Add(rule);
                    break;
                case RuleType.BeforeStartGame:
                    RulesBeforeGameStartCashed.Add(rule);
                    break;
                case RuleType.AfterEndOfCycle:
                    RulesAfterCycleEndedCashed.Add(rule);
                    break;
            }
        }
    }

    public void AddRule(string ruleId)
    {
        if (_rulesConfigSO.HasRule(ruleId)) Rules.Add(_rulesConfigSO.GetRule(ruleId).ruleInstance);
    }

    public void RemoveRule(string ruleId)
    {
        var ruleToRemove = Rules.Find(r => r.ID == ruleId);
        if (ruleToRemove != null) Rules.Remove(ruleToRemove);
    }

    private void HandleRulesChanged()
    {
        // Кешируем правила когда они поменялись И вызываем событие
        RecalculateCashedRules();
        OnCashedRulesRecalculated?.Invoke();
    }
}
