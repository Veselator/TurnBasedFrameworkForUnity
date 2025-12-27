using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TBS_BeforeTurnEndHandler : MonoBehaviour
{
    // Обработка правил перед концом хода

    // Правила RulesAfterTurnCashed, RulesAfterCycleEndedCashed и RulesToWinOrDefeatCashed

    public static TBS_BeforeTurnEndHandler Instance { get; private set; }
    private TBS_RulesManager _rulesManager;
    private TBS_OrderManager _orderManager;
    private GlobalFlags _globalFlags;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _rulesManager = TBS_RulesManager.Instance;
        _orderManager = TBS_OrderManager.Instance;
        if (_rulesManager.RulesToWinOrDefeatCashed.Count == 0) Debug.LogWarning("Эм, ты в курсе что не получится победить или проиграть????");

        _globalFlags.NextTurnQuery.AddListener(HandleBeforeTurnEnd);
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            _globalFlags.OnTurnStartedPrepared.RemoveListener(HandleBeforeTurnEnd);
        }
    }

    public void HandleBeforeTurnEnd(int turnId, int playerId)
    {
        StartCoroutine(ProcessBeforeTurnEndRules(turnId, playerId));
    }

    // Асинхронная проверка правил
    private IEnumerator ProcessBeforeTurnEndRules(int turnId, int playerId)
    {
        var rulesBeforeEnd = _rulesManager.RulesAfterTurnCashed;
        var rulesOfWinOrLose = _rulesManager.RulesToWinOrDefeatCashed;
        var rulesAfterCycle = _rulesManager.RulesAfterCycleEndedCashed;

        // Проверяем, есть ли правила перед концом хода
        if (rulesBeforeEnd.Count != 0)
        {
            foreach (IRule rule in rulesBeforeEnd)
            {
                yield return StartCoroutine(rule.ExecuteRule(turnId, playerId));
            }
        }

        // Проверяем, конец ли круга игроков и есть ли соотвествующие правила
        if (rulesAfterCycle.Count != 0 && _orderManager.IsTurnEndsCycle(turnId))
        {
            foreach (IRule rule in rulesAfterCycle)
            {
                yield return StartCoroutine(rule.ExecuteRule(turnId, playerId));
            }
        }

        // Проверяем условия победы

        if (rulesOfWinOrLose.Count != 0)
        {
            foreach (RuleToWinOrDefeat rule in rulesOfWinOrLose)
            {
                RuleWinResult result = rule.CheckIsAnybodyWon();
                if (result.isWin)
                {
                    // Есть победитель
                    _globalFlags.TriggerOnRoundEnded(result.winnerPlayerID);
                    yield break;
                }
                else if (result.isDraft)
                {
                    // Ничья
                    _globalFlags.TriggerOnRoundEnded(-1); // Ничья
                    yield break;
                }
            }
        }

        _globalFlags.TriggerAllowNextTurn();
    }
}
