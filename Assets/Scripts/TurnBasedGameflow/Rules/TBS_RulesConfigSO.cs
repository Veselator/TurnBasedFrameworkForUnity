using System;
using System.Collections.Generic;
using UnityEngine;

// Основной конфиг ВСЕХ доступных правил в игре
[CreateAssetMenu(fileName = "TBS_RulesConfigSO", menuName = "TurnBasedGameflow/TBS_RulesConfigSO")]
public class TBS_RulesConfigSO : ScriptableObject
{
    // Конфиг всех доступных правил 
    [SerializeField]
    private List<RuleEntry> ruleEntries = new List<RuleEntry>();

    private Dictionary<string, RuleSO> _rulesCache;

    public Dictionary<string, RuleSO> Rules
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
        _rulesCache = new Dictionary<string, RuleSO>();
        foreach (var entry in ruleEntries)
        {
            if (!string.IsNullOrEmpty(entry.ruleID) && entry.rule != null)
            {
                _rulesCache[entry.ruleID] = entry.rule;
                entry.rule.ID = entry.ruleID; // Важно что-бы id соотвествовал ключу
            }
        }
    }

    public RuleSO GetRule(string ruleID)
    {
        if (Rules.TryGetValue(ruleID, out RuleSO rule))
        {
            return rule;
        }
        return null;
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
    public RuleSO rule;
    public int priority;
}