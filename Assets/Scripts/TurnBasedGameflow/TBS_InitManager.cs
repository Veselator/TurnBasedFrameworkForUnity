using System;
using UnityEngine;

public class TBS_InitManager : MonoBehaviour
{
    // Отвечает за корректную инициализацию всех систем в рамках пошаговой системы

    private GlobalFlags _globalFlags;
    [SerializeField] private TBS_InitConfigSO _initConfig;

    private void Start()
    {
        // Временно (возможно (наверное))
        Init();
    }

    public void Init()
    {
        // Глобальные флаги - обязательно
        _globalFlags = new GlobalFlags();
        
        // Отлов ошибок, когда нужного компонента системы нет!

        // Инициализируем игроков
        if (TBS_PlayersManager.Instance != null)
        {
            TBS_PlayersManager.Instance.Init(_globalFlags, _initConfig);
        }
        else
        {
            Debug.LogError("Where is TBS_PlayersManager?! I`m veeeery angry >:| ");
            return;
        }

        // После игроков инициализируем порядок ихнего хода
        if (TBS_OrderManager.Instance != null)
        {
            TBS_OrderManager.Instance.Init(_globalFlags, _initConfig);
        }
        else
        {
            Debug.LogError("Where is TBS_OrderManager?! I`m so sad without it :( ");
            return;
        }

        // Инициализируем правила
        if (TBS_RulesManager.Instance != null)
        {
            TBS_RulesManager.Instance.Init(_globalFlags, _initConfig);
        } 
        else
        {
            Debug.LogError("Where is TBS_RulesManager?! I WANT IT! ");
            return;
        }

        // Инициализируем карту
        if (TBS_BaseMap.Instance != null)
        {
            TBS_BaseMap.Instance.Init();
        }
        else
        {
            Debug.LogError("Where is TBS_BaseMap?! Please, make it! ");
            return;
        }

        // Дальше Handlers - держатель до начала и после конца хода; после конца раунда
        if (TBS_BeforeTurnStartHandler.Instance != null)
        {
            TBS_BeforeTurnStartHandler.Instance.Init(_globalFlags);
        }
        else
        {
            Debug.LogError("Where is TBS_BeforeTurnStartHandler?! I want this one ");
            return;
        }
        
        if (TBS_BeforeTurnEndHandler.Instance != null)
        {
            TBS_BeforeTurnEndHandler.Instance.Init(_globalFlags);
        }
        else
        {
            Debug.LogError("Where is TBS_BeforeTurnEndHandler?! You make me unhappy ");
            return;
        }
        
        if (TBS_RoundEndHandler.Instance != null)
        {
            TBS_RoundEndHandler.Instance.Init(_globalFlags);
        }
        else
        {
            Debug.LogError("Where is TBS_RoundEndHandler?! I. Want. It. NOW! ");
            return;
        }

        // Инициализируем менеджер ходов
        if (TBS_TurnsManager.Instance != null)
        {
            TBS_TurnsManager.Instance.Init(_globalFlags, _initConfig);
        }
        else
        {
            Debug.LogError("Where is TBS_TurnsManager?! He is VERY important part of turn based system ");
            return;
        }

        // Начали!
        TBS_TurnsManager.Instance.StartGame();
    }
}
