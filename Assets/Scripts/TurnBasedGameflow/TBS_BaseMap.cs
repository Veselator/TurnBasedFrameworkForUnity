using System.Collections;
using UnityEngine;

public abstract class TBS_BaseMap : MonoBehaviour
{
    // Базовый класс карты пошаговой системы
    // Хранит информацию о текущем состоянии карты

    public static TBS_BaseMap Instance { get; private set; }
    public abstract IEnumerable Map { get; }

    private void Awake()
    {
        Instance = this;
    }

    public abstract void Init(TBS_InitConfigSO config);
    public abstract void Reload();
}
