using UnityEngine.Events;

public class GlobalFlags
{
    // Шина событий
    // Название глобальные флаги - единственное, что осталось от прошлой версии реализации
    // То, как она была реализована до этого можно посмотреть в репозитории CourseWork2025

    public UnityEvent<int> NextStepQuery { get; } = new(); // Запрос следующего шага
    public UnityEvent<int, int> OnTurnStartedPrepared { get; } = new(); // ПЕРЕД тем, как начинается ход
    public UnityEvent<int, int> OnTurnStarted { get; } = new(); // Когда начинается ход
    public UnityEvent<int, int> OnTurnEnded { get; } = new(); // Когда заканчивается ход
    public UnityEvent AllowNextStep { get; } = new(); // Разрешение на следующий шаг

    public void TriggerNextStepQuery(int stepId)
    {
        NextStepQuery.Invoke(stepId);
    }

    public void TriggerOnTurnStarted(int turnId, int playerId)
    {
        // playerId - ID игрока, чей ход начинается
        OnTurnStarted.Invoke(turnId, playerId);
    }
    public void TriggerOnTurnStartedPrepared(int turnId, int playerId)
    {
        // playerId - ID игрока, чей ход начинается
        OnTurnStartedPrepared.Invoke(turnId, playerId);
    }

    public void TriggerOnTurnEnded(int turnId, int playerId)
    {
        // playerId - ID игрока, чей ход кончается
        OnTurnEnded.Invoke(turnId, playerId);
    }

    public void TriggerAllowNextStep()
    {
        AllowNextStep.Invoke();
    }
}