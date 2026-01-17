using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_RoundEndHandler : MonoBehaviour
{
    // Держатель конца раунда
    // Отвечает за перезагрузку данных
    // и подготовку к следующему раунду

    // Правила AfterRound

    public static TBS_RoundEndHandler Instance {  get; private set; }
    private GlobalFlags _globalFlags;

    private TBS_BaseMap _map;
    private TBS_PlayersManager _playersManager;
    private TBS_RulesManager _rulesManager;
    private TBS_TurnsManager _turnsManager;
    private TBS_OrderManager _orderManager;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _globalFlags.NextRoundQuery.AddListener(HandleRoundQuery);

        _map = TBS_BaseMap.Instance;
        _playersManager = TBS_PlayersManager.Instance;
        _rulesManager = TBS_RulesManager.Instance;
        _turnsManager = TBS_TurnsManager.Instance;
        _orderManager = TBS_OrderManager.Instance;
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            _globalFlags.NextRoundQuery.RemoveListener(HandleRoundQuery);
        }
    }

    private void HandleRoundQuery(int roundResult, RuleWinResult result)
    {
        // Перезагрузка данных, проверка правил AfterRound
        StartCoroutine(HandleRoundQuery(result));
    }

    private IEnumerator HandleRoundQuery(RuleWinResult result)
    {
        _playersManager.ResetPlayersPoints();
        _map.Reload();
        _orderManager.Reload();

        List<RuleAfterEndOfRound> rules = _rulesManager.RulesAfterEndOfRoundCashed;

        if (rules.Count != 0)
        {
            foreach (var rule in rules)
            {
                yield return rule.ExecuteRule(_turnsManager.CurrentTurn, result);
            }
        }

        _globalFlags.TriggerNextRoundAllowed();
    }
}
