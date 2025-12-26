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
    public UnityEvent OnGameStarted { get; } = new(); // Игра началась

    // int - id победившего игрока; -1 - ничья
    public UnityEvent<int> OnGameEnded { get; } = new(); // Игра кончилась

    // Порядок вызова (важно!):

    // OnGameStarted                Игра началась

    // Каждый ход:
    //      OnTurnStartedPrepared        Подготовка к началу хода
    //      OnTurnStarted                Непосредственное начало хода - игрок может действовать
    //      OnTurnEnded                  Игрок сделал ход
    //      NextTurnQuery                Запрашиваем следующий ход И проверяем условия победы/поражения
    //      OnNextTurnAllowed            Разрешаем следующий ход

    // OnGameEnded

    // Есть вопрос к необходимости этих событий
    // Можно просто отслеживать через OnTurnStarted и OnTurnEnded
    // Но так будет проще и оптимиизированнее - что-бы объекты отслеживали именно ход игрока, что важно для интерфейса
    public UnityEvent OnHumansTurnStarted { get; } = new(); // Ход игрока начался
    public UnityEvent OnHumansTurnEnded { get; } = new(); // Ход игрока закончился

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

    public void TriggerOnGameStarted()
    {
        OnGameStarted?.Invoke();
    }

    public void TriggerOnGameEnded(int playerWhoWonId)
    {
        OnGameEnded?.Invoke(playerWhoWonId);
    }

    public void TriggerOnHumansTurnStarted()
    {
        OnHumansTurnStarted?.Invoke();
    }

    public void TriggerOnHumansTurnEnded()
    {
        OnHumansTurnEnded?.Invoke();
    }
}