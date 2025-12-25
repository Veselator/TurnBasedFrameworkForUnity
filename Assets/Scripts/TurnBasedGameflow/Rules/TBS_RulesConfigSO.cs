using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TBS_RulesConfigSO", menuName = "TurnBasedGameflow/TBS_RulesConfigSO")]
public class TBS_RulesConfigSO : ScriptableObject
{
    // Конфигурация всех возможных правил

    [SerializeField]
    private List<RuleEntry> ruleEntries = new List<RuleEntry>();

    // Словарь для быстрого доступа (создаётся при загрузке)
    private Dictionary<string, RuleDefinition> _rulesCache;

    public Dictionary<string, RuleDefinition> Rules
    {
        get
        {
            if (_rulesCache == null)
            {
                BuildCache();
            }
            return _rulesCache;
        }
    }

    private void OnEnable()
    {
        BuildCache();
    }

    private void BuildCache()
    {
        _rulesCache = new Dictionary<string, RuleDefinition>();
        foreach (var entry in ruleEntries)
        {
            if (!string.IsNullOrEmpty(entry.ruleID))
            {
                if (_rulesCache.ContainsKey(entry.ruleID))
                {
                    Debug.LogWarning($"Duplicate ruleID found: {entry.ruleID}. Skipping.");
                    continue;
                }

                _rulesCache[entry.ruleID] = entry.definition;
                entry.definition.ruleInstance.ID = entry.ruleID;
            }
        }
    }

    // Вспомогательный метод для получения правила
    public RuleDefinition GetRule(string ruleID)
    {
        if (Rules.TryGetValue(ruleID, out RuleDefinition rule))
        {
            return rule;
        }

        return default;
    }

    public bool HasRule(string ruleID)
    {
        return Rules.ContainsKey(ruleID);
    }
}

[Serializable]
public class RuleEntry
{
    public string ruleID;
    public RuleDefinition definition;
}

[Serializable]
public struct RuleDefinition
{
    public RuleType ruleType;

    [SerializeReference]
    public IRule ruleInstance;

    public int priority;
}

public enum RuleType
{
    BeforeTurn,
    AfterTurn,
    ToCalculatePoints,
    ToWinOrDefeat,
    ToAllowAction,
    BeforeStartGame
}