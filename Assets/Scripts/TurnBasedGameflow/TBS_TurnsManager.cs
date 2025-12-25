using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_TurnsManager : MonoBehaviour
{
    // Менеджер ходов пошаговой системы
    // Отвечает за то, что-бы ходы корректно сменялись

    public static TBS_TurnsManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
}
