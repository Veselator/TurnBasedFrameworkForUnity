using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_OrderManager : MonoBehaviour
{
    // Менеджер коректной инициализации порядка хода
    // А также за учёт того, кто сейчас должен ходить
    // Необходим для гибкости в выборе способа инициализации порядка хода

    public static TBS_OrderManager Instance { get; private set; }
    public List<int> order; // Порядок ходов, хранит ID игроков в порядке их ходов
    private TBS_BaseOrderRule orderRule; // Правило формирования порядка ходов
    public int CurrentTurnIndex { get; private set; } = 0; // Индекс текущего хода в списке order
    public int CurrentPlayerID => order[CurrentTurnIndex]; // ID игрока, который ходит сейчас

    private void Awake()
    {
        Instance = this;
        // DI через инспектор
        // Могут быть разные правила формирования порядка хода
        orderRule = GetComponent<TBS_BaseOrderRule>();
    }

    public void Init()
    {
        order = orderRule.GetTurnOrder(TBS_PlayersManager.Instance.Players);
    }

    public void NextPlayer()
    {
        CurrentTurnIndex = (CurrentTurnIndex + 1) % order.Count;
    }
}
