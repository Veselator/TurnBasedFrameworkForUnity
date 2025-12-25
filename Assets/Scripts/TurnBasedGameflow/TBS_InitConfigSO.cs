using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TBS_InitConfigSO", menuName = "TurnBasedGameflow/TBS_InitConfigSO")]
public class TBS_InitConfigSO : ScriptableObject
{
    // Конфиг пошаговой системы
    // Инициализирует конкретную игровую сессию
    public PlayerInfo[] players;
}

[Serializable]
public struct PlayerInfo
{
    public string playerName;
    public bool isAI;
}