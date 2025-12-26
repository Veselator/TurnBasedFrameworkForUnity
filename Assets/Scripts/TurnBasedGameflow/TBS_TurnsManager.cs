using UnityEngine;

public class TBS_TurnsManager : MonoBehaviour
{
    // Менеджер ходов пошаговой системы
    // Отвечает за то, что-бы ходы корректно сменялись

    public static TBS_TurnsManager Instance { get; private set; }
    private GlobalFlags _globalFlags;
    private TBS_OrderManager _orderManager;
    private TBS_PlayersManager _players;

    private bool _isNextTurnQueuedFlag = false;
    private int _currentTurn = 0;
    public int CurrentTurn => _currentTurn;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _globalFlags.OnNextTurnAllowed.AddListener(OnNextTurnAllowed);
        _globalFlags.OnTurnEnded.AddListener(HandleEndOfTurn);
        _globalFlags.OnTurnStarted.AddListener(OnTurnStarted);

        _orderManager = TBS_OrderManager.Instance;
        _players = TBS_PlayersManager.Instance;
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            // Отписка
            _globalFlags.OnNextTurnAllowed.RemoveListener(OnNextTurnAllowed);
            _globalFlags.OnTurnEnded.RemoveListener(HandleEndOfTurn);
            _globalFlags.OnTurnStarted.RemoveListener(OnTurnStarted);
        }
    }

    // Начать игру
    public void StartGame()
    {
        _globalFlags.TriggerOnGameStarted();
        // Асинхронное начало хода? Надо подумать
        _globalFlags.TriggerOnTurnStartedPrepared(_currentTurn, _orderManager.CurrentPlayerID);
    }

    public void HandleEndOfTurn(int turnId, int playerId)
    {
        _isNextTurnQueuedFlag = true;
        _globalFlags.TriggerNextTurnQuery(turnId, playerId); // Запрос на следующий ход
    }

    public void OnNextTurnAllowed()
    {
        if (!_isNextTurnQueuedFlag) return;
        _isNextTurnQueuedFlag = false;
        _currentTurn++;
        _orderManager.NextPlayer();

        _globalFlags.TriggerOnTurnStartedPrepared(_currentTurn, _orderManager.CurrentPlayerID);
    }

    public void OnTurnStarted(int turnId, int playerId)
    {
        // Начало хода
        IPlayer currentPlayer = _players.GetPlayerByID(playerId);
        currentPlayer.Act();
        if (!currentPlayer.IsAI) _globalFlags.TriggerOnHumansTurnStarted();
    }
}
