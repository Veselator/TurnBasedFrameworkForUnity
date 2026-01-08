using System;
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
    public IPlayer CurrentPlayer => _players.GetPlayerByID(_orderManager.CurrentPlayerID);

    private bool _areInfinityTurns;
    private int _maxTurns;

    // Можна было бы сделать отдельный менеджер раундов
    // Так как получается, что немного нарушается SRP
    // Но как-будто overengineering для данной задачи
    private int _currentRound = 0;
    public int CurrentRound => _currentRound;
    private int _maxRounds;
    public int MaxRounds => _maxRounds;
    private bool _isNextRoundQueuedFlag = false;

    public event Action<int> OnTurnChanged;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags, TBS_InitConfigSO config)
    {
        _globalFlags = globalFlags;

        _areInfinityTurns = config.areTurnsInfinite;
        _maxTurns = config.maxTurnsCount;
        _maxRounds = config.maxRoundsCount;

        _globalFlags.OnNextTurnAllowed.AddListener(OnNextTurnAllowed);
        _globalFlags.OnTurnEnded.AddListener(HandleEndOfTurn);
        _globalFlags.OnTurnStarted.AddListener(OnTurnStarted);
        _globalFlags.OnRoundEnded.AddListener(OnRoundEnded);
        _globalFlags.NextRoundAllowed.AddListener(OnNextRoundAllowed);
        _globalFlags.OnGameStartedAllowed.AddListener(OnGameStartAllowed);

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
            _globalFlags.OnRoundEnded.RemoveListener(OnRoundEnded);
            _globalFlags.NextRoundAllowed.RemoveListener(OnNextRoundAllowed);
            _globalFlags.OnGameStartedAllowed.AddListener(OnGameStartAllowed);
        }
    }

    // Начать игру
    public void StartGame()
    {
        _globalFlags.TriggerOnGameStartedQuery();
    }

    private void OnGameStartAllowed()
    {
        _globalFlags.TriggerOnRoundStarted(_currentTurn);
        _globalFlags.TriggerOnTurnStartedPrepared(_currentTurn, _orderManager.CurrentPlayerID);

        OnTurnChanged?.Invoke(CurrentTurn);
    }

    // Дальше - обработчики событий

    private void HandleEndOfTurn(int turnId, int playerId)
    {
        _isNextTurnQueuedFlag = true;
        if (!_players.IsPlayerAi(playerId)) _globalFlags.TriggerOnHumansTurnEnded(playerId);
        _globalFlags.TriggerNextTurnQuery(turnId, playerId); // Запрос на следующий ход
    }

    private void OnNextTurnAllowed()
    {
        if (!_isNextTurnQueuedFlag) return;
        _isNextTurnQueuedFlag = false;

        if (!_areInfinityTurns)
        {
            if(_currentTurn >= _maxTurns)
            {
                // Раунд закончился
                // НО
                // Если мы дошли до этого этапа - значит, победителя нет
                // Значит ничья
                _globalFlags.TriggerOnRoundEnded(new RuleWinResult(GameWinCheckResult.Draft));
                return;
            }
        }

        _currentTurn++;
        OnTurnChanged?.Invoke(CurrentTurn);
        _orderManager.NextPlayer();

        _globalFlags.TriggerOnTurnStartedPrepared(_currentTurn, _orderManager.CurrentPlayerID);
    }

    private void OnTurnStarted(int turnId, int playerId)
    {
        // Начало хода
        IPlayer currentPlayer = _players.GetPlayerByID(playerId);
        currentPlayer.Act();
        OnTurnChanged?.Invoke(CurrentTurn);
        if (!currentPlayer.IsAI) _globalFlags.TriggerOnHumansTurnStarted(playerId);
    }

    // Нарушение SRP? Возможно
    private void OnRoundEnded(RuleWinResult result)
    {
        // playerId = -1 - ничья
        // Как вараинт правила добавления очков?
        if (result.Result == GameWinCheckResult.Win) _players.AddOverallScoreToPlayerWithId(result.WinnerPlayerID, 1);

        _currentRound++;
        if (_currentRound >= _maxRounds)
        {
            // Всё, игра закончилась
            _globalFlags.TriggerOnGameEnded(_players.GetPlayersOut());
            return;
        }

        // Запрос для начала нового раунда
        _isNextRoundQueuedFlag = true;
        _globalFlags.TriggerNextRoundQuery(_currentRound);
    }

    private void OnNextRoundAllowed()
    {
        if (!_isNextRoundQueuedFlag) return;
        _isNextRoundQueuedFlag = false;

        _currentTurn = 0;
        OnTurnChanged?.Invoke(CurrentTurn);

        _globalFlags.TriggerOnRoundStarted(_currentRound);
        _globalFlags.TriggerOnTurnStartedPrepared(_currentTurn, _orderManager.CurrentPlayerID);
    }
}