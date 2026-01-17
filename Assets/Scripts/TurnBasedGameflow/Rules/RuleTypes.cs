using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// Содержит определения всех категорий
// Разные типы правил = разные типы взаимодействий

public abstract class RuleSO : ScriptableObject, IRule
{
    private string id;
    [HideInInspector] public string ID { get => id; set => id = value; }
    public abstract RuleType ruleType { get; }

    public virtual void Init() { }
    public virtual IEnumerator ExecuteRule()
    {
        yield return null;
    }

    public virtual IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield return null;
    }
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
    public abstract IEnumerable CalculatePoints(int turnId, int playerId, TBS_Context context);
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
    public abstract bool IsActionAllowed(params int[] parameters);
    public abstract bool IsActionAllowed(IEnumerable parameters);
}

public abstract class RuleToWinOrDefeat : RuleSO
{
    public override RuleType ruleType => RuleType.ToWinOrDefeat;
    // Проверка, победил ли кто-то
    // Возвращает RuleWinResult, который содержит ID победителя, если таковой есть
    // -1 - ничья

    //public abstract RuleWinResult CheckIsPlayerWon(int playerId);

    // Расчёт С контекстом
    public abstract RuleWinResult CheckIsPlayerWon(int playerId, TBS_Context context = null);

    // Проблема - как сделать так, что-бы можно было предсказывать результат победы без изменения поля?
    // Создавать новый экземпляр всего поля для каждой проверки каждого правила= утечка памяти
    // Значит, нужно сделать контекст
}

// Класс-запись, который хранит информацию о результате проверки победы
public class RuleWinResult
{
    // Должен сохранять порядок результатов проверки?! Первое место, второе и тд?
    public GameWinCheckResult Result { get; private set; }
    public int WinnerPlayerID { get; private set; }

    public RuleWinResult() : this(GameWinCheckResult.None, -1) { }

    public RuleWinResult(GameWinCheckResult res) : this (res, -1) { }

    public RuleWinResult(GameWinCheckResult res, int playerId)
    {
        Result = res;
        WinnerPlayerID = playerId;
    }

    // Перегрузка операторов
    public static bool operator ==(RuleWinResult res1, RuleWinResult res2)
    {
        return res1.Result == res2.Result;
    }

    public static bool operator !=(RuleWinResult res1, RuleWinResult res2)
    {
        return res1.Result != res2.Result;
    }

    public static bool operator ==(RuleWinResult res1, GameWinCheckResult res2)
    {
        return res1.Result == res2;
    }

    public static bool operator !=(RuleWinResult res1, GameWinCheckResult res2)
    {
        return res1.Result != res2;
    }

    public override bool Equals(object obj)
    {
        return this == (RuleWinResult)obj;
    }

    public override int GetHashCode()
    {
        return Result.GetHashCode() + WinnerPlayerID.GetHashCode();
    }
}

public enum GameWinCheckResult
{
    None,
    Draft,
    Win
}

public abstract class RuleAfterCycleEnd : RuleSO
{
    public override RuleType ruleType => RuleType.AfterEndOfCycle;
}

public abstract class RuleAfterEndOfRound : RuleSO
{
    public override RuleType ruleType => RuleType.AfterEndOfRound;
    public abstract IEnumerator ExecuteRule(int roundId, RuleWinResult result);
}

public abstract class RuleBeforeEndOfGame : RuleSO
{
    public override RuleType ruleType => RuleType.AfterEndOfGame;
    public abstract IEnumerator ExecuteRule(List<IPlayer> players);
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
    AfterEndOfRound,
    AfterEndOfGame
}