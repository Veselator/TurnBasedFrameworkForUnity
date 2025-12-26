using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TBS_InitManager : MonoBehaviour
{
    // Отвечает за корректную инициализацию всех систем в рамках пошаговой системы

    private GlobalFlags _globalFlags;
    [SerializeField] private TBS_InitConfigSO _initConfig;

    private void Start()
    {
        // Временно
        Init();
    }

    public void Init()
    {
        _globalFlags = new GlobalFlags();
        TBS_PlayersManager.Instance.Init(_globalFlags, _initConfig);
        TBS_OrderManager.Instance.Init(_globalFlags);
        TBS_RulesManager.Instance.Init(_globalFlags, _initConfig);
        TBS_BaseMap.Instance.Init();
        TBS_BeforeTurnStartHandler.Instance.Init(_globalFlags);

        TBS_TurnsManager.Instance.Init(_globalFlags);
    }
}
