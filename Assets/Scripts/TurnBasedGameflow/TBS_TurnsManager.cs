using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_TurnsManager : MonoBehaviour
{
    // Менеджер ходов пошаговой системы
    // Отвечает за то, что-бы ходы корректно сменялись

    public static TBS_TurnsManager Instance { get; private set; }
    private GlobalFlags _globalFlags;
    private TBS_OrderManager _orderManager;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _globalFlags.OnNextStepAllowed.AddListener(HandleEndOfTurn);

        _orderManager = TBS_OrderManager.Instance;
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            
        }
    }

    // Начать игру
    public void StartGame()
    {
        
    }

    public void HandleEndOfTurn()
    {

    }
}
