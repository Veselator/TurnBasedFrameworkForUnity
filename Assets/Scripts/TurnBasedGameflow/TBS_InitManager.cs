using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class TBS_InitManager : MonoBehaviour
{
    // Отвечает за корректную инициализацию всех систем в рамках пошаговой системы
    private GlobalFlags _globalFlags;
    [SerializeField] private TBS_InitConfigSO _initConfig;

    public void Init()
    {
        _globalFlags = new GlobalFlags();
        TBS_PlayersManager.Instance.Init(_globalFlags, _initConfig);
    }
}
