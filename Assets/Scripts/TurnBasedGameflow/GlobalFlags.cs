using System.Collections.Generic;
using UnityEngine.Events;

public class GlobalFlags
{
    // Шина событий
    // Название глобальные флаги - единственное, что осталось от прошлой версии реализации
    // То, как она была реализована до этого можно посмотреть в репозитории CourseWork2025

    // Turn based system события!!!!!!!!!!!!!

    // Везде
    // Первый int - номер хода
    // Второй - ID игрока

    public UnityEvent<int, int> NextTurnQuery { get; } = new(); // Запрос следующего шага
    public UnityEvent<int, int> OnTurnStartedPrepared { get; } = new(); // ПЕРЕД тем, как начинается ход; подготовка к нему
    public UnityEvent<int, int> OnTurnStarted { get; } = new(); // Когда начинается ход
    public UnityEvent<int, int> OnTurnEnded { get; } = new(); // Когда заканчивается ход
    // если игрок - то значит будет вызываться в интерфейсе, после нажатия кнопки условно
    // если ИИ - то сразу после выполнения его действий

    public UnityEvent OnNextTurnAllowed { get; } = new(); // Разрешение на следующий шаг
    public UnityEvent OnFullCycleEnded { get; } = new(); // Прошли круг и вернулись к первому игроку
    public UnityEvent OnGameStartedQuery { get; } = new(); // Запрос на начало игры
    public UnityEvent OnGameStartedAllowed { get; } = new(); // Игра началась

    // int - id ТЕКУЩЕГО раунда
    public UnityEvent<int> OnRoundStarted { get; } = new(); // Раунд начался
    // Но тут int - id победившего игрока или -1
    public UnityEvent<int> OnRoundEnded { get; } = new(); // Раунд закончился

    public UnityEvent<int> NextRoundQuery { get; } = new(); // Запрос на следующий раунд
    public UnityEvent NextRoundAllowed { get; } = new(); // Запрос на следующий раунд

    // int - id победившего игрока; -1 - ничья
    public UnityEvent<List<IPlayer>> OnGameEnded { get; } = new(); // Игра кончилась

    // ------------------------------------------------------------------------

    // Порядок вызова (важно!):

    // OnGameStartedQuery                       Запрос на начало игры
    // OnGameStarted                            Игра началась

    // Каждый раунд:
    //      OnRoundStarted                      Раунд начался (НЕ КРУГ)

    //      Каждый ход:
    //            OnTurnStartedPrepared         Подготовка к началу хода
    //            OnTurnStarted                 Непосредственное начало хода - игрок может действовать
    //            OnTurnEnded                   Игрок сделал ход
    //            NextTurnQuery                 Запрашиваем следующий ход И проверяем условия победы/поражения
    //            OnNextTurnAllowed             Разрешаем следующий ход

    //      OnRoundEnded                        Раунд (НЕ КРУГ!!!!) окончен - есть победитель либо ничья
    //      NextRoundQuery
    //      NextRoundAllowed

    // OnGameEnded

    // ------------------------------------------------------------------------

    // Есть вопрос к необходимости этих событий
    // Можно просто отслеживать через OnTurnStarted и OnTurnEnded
    // Но так будет проще и оптимизированнее - что-бы объекты отслеживали именно ход игрока, что важно для интерфейса
    public UnityEvent<int> OnHumansTurnStarted { get; } = new(); // Ход игрока начался
    public UnityEvent<int> OnHumansTurnEnded { get; } = new(); // Ход игрока закончился

    public void TriggerNextTurnQuery(int stepId, int playerId)
    {
        NextTurnQuery?.Invoke(stepId, playerId);
    }

    public void TriggerOnTurnStarted(int turnId, int playerId)
    {
        // playerId - ID игрока, чей ход начинается
        OnTurnStarted?.Invoke(turnId, playerId);
    }
    public void TriggerOnTurnStartedPrepared(int turnId, int playerId)
    {
        // playerId - ID игрока, чей ход начинается
        OnTurnStartedPrepared?.Invoke(turnId, playerId);
    }

    public void TriggerOnTurnEnded(int turnId, int playerId)
    {
        // playerId - ID игрока, чей ход кончается
        OnTurnEnded?.Invoke(turnId, playerId);
    }

    public void TriggerAllowNextTurn()
    {
        OnNextTurnAllowed?.Invoke();
    }

    public void TriggerOnFullCycleEnded()
    {
        OnFullCycleEnded?.Invoke();
    }

    public void TriggerOnGameStartedQuery()
    {
        OnGameStartedQuery?.Invoke();
    }

    public void TriggerOnGameStartedAllowed()
    {
        OnGameStartedAllowed?.Invoke();
    }

    public void TriggerOnRoundStarted(int roundId)
    {
        OnRoundStarted?.Invoke(roundId);
    }

    public void TriggerOnRoundEnded(int playerWhoWonId)
    {
        OnRoundEnded?.Invoke(playerWhoWonId);
    }

    public void TriggerOnGameEnded(List<IPlayer> players)
    {
        OnGameEnded?.Invoke(players);
    }

    public void TriggerOnHumansTurnStarted(int playerId)
    {
        OnHumansTurnStarted?.Invoke(playerId);
    }

    public void TriggerOnHumansTurnEnded(int playerId)
    {
        OnHumansTurnEnded?.Invoke(playerId);
    }

    public void TriggerNextRoundQuery(int roundId)
    {
        NextRoundQuery?.Invoke(roundId);
    }

    public void TriggerNextRoundAllowed()
    {
        NextRoundAllowed?.Invoke();
    }
}