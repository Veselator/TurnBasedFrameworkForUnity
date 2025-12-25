using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TBS_RulesConfigSO", menuName = "TurnBasedGameflow/TBS_RulesConfigSO")]
public class TBS_CurrentRulesConfigSO : ScriptableObject
{
    // Хранит конфигурацию правил текущей игры
    public LinkedRuleInfo[] rules;
}

[Serializable]
public struct LinkedRuleInfo
{
    public string linkedRuleID;
}