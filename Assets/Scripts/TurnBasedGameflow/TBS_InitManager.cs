using UnityEngine;

public class TBS_InitManager : MonoBehaviour
{
    // Отвечает за корректную инициализацию всех систем в рамках пошаговой системы

    private GlobalFlags _globalFlags;
    [SerializeField] private TBS_InitConfigSO _initConfig;

    private void Start()
    {
        // Временно (возможно)
        Init();
    }

    public void Init()
    {
        // Глобальные флаги - обязательно
        _globalFlags = new GlobalFlags();

        // Инициализируем игроков
        TBS_PlayersManager.Instance.Init(_globalFlags, _initConfig);
        // После игроков инициализируем порядок ихнего хода
        TBS_OrderManager.Instance.Init(_globalFlags);
        // Инициализируем правила
        TBS_RulesManager.Instance.Init(_globalFlags, _initConfig);
        // Инициализируем карту
        TBS_BaseMap.Instance.Init();
        // Дальше Handlers - держатель до начала и после конца
        TBS_BeforeTurnStartHandler.Instance.Init(_globalFlags);
        TBS_BeforeTurnEndHandler.Instance.Init(_globalFlags);

        // Инициализируем менеджер ходов
        TBS_TurnsManager.Instance.Init(_globalFlags);

        // Начали!
        TBS_TurnsManager.Instance.StartGame();
    }
}
