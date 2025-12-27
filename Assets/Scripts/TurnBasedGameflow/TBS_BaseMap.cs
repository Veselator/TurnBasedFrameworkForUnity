using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TBS_BaseMap : MonoBehaviour
{
    // Базовый класс карты пошаговой системы
    public static TBS_BaseMap Instance { get; private set; }
    public abstract IEnumerable Map { get; }

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        
    }
}
