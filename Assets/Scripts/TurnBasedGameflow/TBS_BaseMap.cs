using System.Collections;
using UnityEngine;

public abstract class TBS_BaseMap : MonoBehaviour
{
    // Базовый класс карты пошаговой системы
    // Хранит информацию о текущем состоянии карты

    public static TBS_BaseMap Instance { get; private set; }
    // Карта для внещней области видимости
    public abstract IEnumerable Map { get; }
    // Название не совсем понятное. Короче, это последний элемент, который был добавлен
    // Например, последний камень
    public abstract object LastModifiedThing { get; }

    private void Awake()
    {
        Instance = this;
    }

    public abstract void Init(TBS_InitConfigSO config);
    public abstract void Reload();
}
