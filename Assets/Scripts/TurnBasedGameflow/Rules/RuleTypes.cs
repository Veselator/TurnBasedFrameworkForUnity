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
    // В теории принцип работы таков:
    // TBS_BeforeTurnEndHandler проходится по каждому правилу
    // На вход передаёт список - карта
    // Пропускаем список через все методы
    // На выходе суммируем элементы списка - так и получаем число очков за действие

    public override RuleType ruleType => RuleType.ToCalculatePoints;
    public abstract IEnumerable CalculatePoints(int turnId, int playerId);
    public abstract IEnumerable CalculatePoints(int turnId, int playerId, IEnumerable newMapState);
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
    public override RuleType ruleType => RuleType.ToAllowAction;
    // TODO: что-то придумать
    public abstract bool IsActionAllowed(params int[] parameters);
    public abstract bool IsActionAllowed(IEnumerable parameters);
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