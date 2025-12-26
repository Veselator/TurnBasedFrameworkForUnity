using System.Collections;
using UnityEngine;

// —одержит определени€ всех категорий
// –азные типы правил = разные типы взаимодействий

public abstract class RuleSO : ScriptableObject, IRule
{
    private string id;
    [HideInInspector] public string ID { get => id; set => id = value; }
    public abstract RuleType ruleType { get; }

    public abstract IEnumerator ExecuteRule(int turnId, int playerId);
}

public abstract class RuleBeforeGameStartSO : RuleSO
{
    public override RuleType ruleType => RuleType.BeforeStartGame;
}

public abstract class RuleToCalculatePoints : RuleSO
{
    // ѕока не совсем понимаю как это будет работать
    public override RuleType ruleType => RuleType.ToCalculatePoints;
    public abstract IEnumerator CalculatePoints(int turnId, int playerId, TBS_BaseMap newMapState);
}

public enum RuleType
{
    BeforeTurn,
    AfterTurn,
    ToCalculatePoints,
    ToWinOrDefeat,
    ToAllowAction,
    BeforeStartGame,
    AfterEndOfCycle
}