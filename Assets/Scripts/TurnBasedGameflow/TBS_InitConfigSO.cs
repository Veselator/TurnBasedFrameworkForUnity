using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TBS_InitConfigSO", menuName = "TurnBasedGameflow/TBS_InitConfigSO")]
public class TBS_InitConfigSO : ScriptableObject
{
    // Конфиг пошаговой системы
    // Инициализирует конкретную игровую сессию
    // Тут много всякого разного, но по названиям и так всё понятно
    // Не маленький - сам разберёшься

    public PlayerInfo[] players;
    public TBS_CurrentRulesConfigSO currentRulesConfig;
    public TBS_RulesConfigSO rulesConfig;

    public bool areTurnsInfinite;
    public int maxTurnsCount;

    public int maxRoundsCount;
}

[Serializable]
public struct PlayerInfo
{
    public string playerName;
    public bool isAI;
}