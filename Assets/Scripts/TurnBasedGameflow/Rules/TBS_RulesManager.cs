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
    public List<RuleBeforeTurn> RulesBeforeTurnCashed { get; private set; } = new();
    public List<RuleBeforeTurnEnd> RulesAfterTurnCashed { get; private set; } = new();
    public List<RuleToCalculatePoints> RulesToCalculatePointsCashed { get; private set; } = new();
    public List<RuleToAllowAction> RulesToAllowActionCashed { get; private set; } = new();
    public List<RuleToWinOrDefeat> RulesToWinOrDefeatCashed { get; private set; } = new();
    public List<RuleBeforeGameStart> RulesBeforeGameStartCashed { get; private set; } = new();
    public List<RuleAfterCycleEnd> RulesAfterCycleEndedCashed { get; private set; } = new();
    public List<RuleAfterEndOfRound> RulesAfterEndOfRoundCashed { get; private set; } = new();

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
            if (_rulesConfigSO.HasRule(ruleId)) _rules.Add(_rulesConfigSO.GetRule(ruleId));
        }

        RecalculateCashedRules();
    }

    private void RecalculateCashedRules()
    {
        // Кешируем правила когда они поменялись
        // Можно оптимизировать и не удалять всё, а просто добавлять и удалять по необходимости

        RulesBeforeTurnCashed.Clear();
        RulesAfterTurnCashed.Clear();
        RulesToCalculatePointsCashed.Clear();
        RulesToAllowActionCashed.Clear();
        RulesToWinOrDefeatCashed.Clear();
        RulesBeforeGameStartCashed.Clear();
        RulesAfterCycleEndedCashed.Clear();
        RulesAfterEndOfRoundCashed.Clear();

        foreach (var rule in Rules)
        {
            switch (rule.ruleType)
            {
                case RuleType.BeforeTurn:
                    RulesBeforeTurnCashed.Add(rule as RuleBeforeTurn);
                    break;
                case RuleType.BeforeTurnEnd:
                    RulesAfterTurnCashed.Add(rule as RuleBeforeTurnEnd);
                    break;
                case RuleType.ToCalculatePoints:
                    RulesToCalculatePointsCashed.Add(rule as RuleToCalculatePoints);
                    break;
                case RuleType.ToAllowAction:
                    RulesToAllowActionCashed.Add(rule as RuleToAllowAction);
                    break;
                case RuleType.ToWinOrDefeat:
                    RulesToWinOrDefeatCashed.Add(rule as RuleToWinOrDefeat);
                    break;
                case RuleType.BeforeStartGame:
                    RulesBeforeGameStartCashed.Add(rule as RuleBeforeGameStart);
                    break;
                case RuleType.AfterEndOfCycle:
                    RulesAfterCycleEndedCashed.Add(rule as RuleAfterCycleEnd);
                    break;
                case RuleType.AfterEndOfRound:
                    RulesAfterEndOfRoundCashed.Add(rule as RuleAfterEndOfRound);
                    break;
            }
        }
    }

    public void AddRule(string ruleId)
    {
        if (_rulesConfigSO.HasRule(ruleId)) Rules.Add(_rulesConfigSO.GetRule(ruleId));
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
