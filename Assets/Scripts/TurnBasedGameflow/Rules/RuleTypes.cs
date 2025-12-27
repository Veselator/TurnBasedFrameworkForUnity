using System.Collections;
using UnityEngine;

// Содержит определения всех категорий
// Разные типы правил = разные типы взаимодействий

public abstract class RuleSO : ScriptableObject, IRule
{
    private string id;
    [HideInInspector] public string ID { get => id; set => id = value; }
    public abstract RuleType ruleType { get; }

    public abstract IEnumerator ExecuteRule(int turnId, int playerId);
}

public abstract class RuleBeforeGameStart : RuleSO
{
    public override RuleType ruleType => RuleType.BeforeStartGame;
}

public abstract class RuleToCalculatePoints : RuleSO
{
    // Пока не совсем понимаю как это будет работать
    // TODO: ДОДЕЛАТЬ СИСТЕМУ
    public override RuleType ruleType => RuleType.ToCalculatePoints;
    public abstract IEnumerator CalculatePoints(int turnId, int playerId, TBS_BaseMap newMapState);
}

public abstract class RuleBeforeTurn : RuleSO
{
    public override RuleType ruleType => RuleType.BeforeTurn;
}

public abstract class RuleBeforeTurnEnd : RuleSO
{
    public override RuleType ruleType => RuleType.BeforeTurnEnd;
}

public abstract class RuleToAllowAction : RuleSO
{
    // TODO: Добавить параметры, которые будут влиять на разрешение действия; продумать логику использования, потому-что пока не совсем понятно
    public override RuleType ruleType => RuleType.ToAllowAction;
}

public abstract class RuleToWinOrDefeat : RuleSO
{
    public override RuleType ruleType => RuleType.ToWinOrDefeat;
    // Проверка, победил ли кто-то
    // Возвращает ID победителя, если таковой есть
    // -1 - ничья

    public abstract RuleWinResult CheckIsAnybodyWon();
}

public struct RuleWinResult
{
    public bool isDraft;
    public bool isWin;
    public int winnerPlayerID;
}

public abstract class RuleAfterCycleEnd : RuleSO
{
    public override RuleType ruleType => RuleType.AfterEndOfCycle;
}

public abstract class RuleAfterEndOfRound : RuleSO
{
    public override RuleType ruleType => RuleType.AfterEndOfRound;
}

public enum RuleType
{
    BeforeTurn,
    BeforeTurnEnd,
    ToCalculatePoints,
    ToWinOrDefeat,
    ToAllowAction,
    BeforeStartGame,
    AfterEndOfCycle,
    AfterEndOfRound
}