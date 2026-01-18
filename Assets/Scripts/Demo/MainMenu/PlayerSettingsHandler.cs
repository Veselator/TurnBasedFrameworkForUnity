using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettingsHandler : MonoBehaviour
{
    // Класс, который содержит информацию о выборе конкретного игрока

    public string Name { get; private set; }
    public bool IsAi { get; private set; }
    public GameObject ChosenPiecePrefab { get; private set; }
    public int AiDifficultyLevel { get; private set; }


}
