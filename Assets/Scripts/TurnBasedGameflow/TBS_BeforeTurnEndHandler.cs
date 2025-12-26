using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TBS_BeforeTurnEndHandler : MonoBehaviour
{
    // Обработка правил перед концом хода

    // Правила RulesAfterTurnCashed и RulesToWinOrDefeatCashed

    public static TBS_BeforeTurnEndHandler Instance { get; private set; }
    private TBS_RulesManager _rulesManager;
    private GlobalFlags _globalFlags;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _rulesManager = TBS_RulesManager.Instance;
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
        List<IRule> rulesBeforeEnd = _rulesManager.RulesAfterTurnCashed;
        List<IRule> rulesOfWinOrLose = _rulesManager.RulesToWinOrDefeatCashed;

        if(rulesBeforeEnd.Count != 0)
        {
            foreach (IRule rule in rulesBeforeEnd)
            {
                yield return StartCoroutine(rule.ExecuteRule(turnId, playerId));
            }
        }
        if(rulesOfWinOrLose.Count != 0)
        {
            // TODO: Проверка победы и поражения
            // OnGameEnded
            foreach (IRule rule in rulesOfWinOrLose)
            {
                yield return StartCoroutine(rule.ExecuteRule(turnId, playerId));
            }
        }

        _globalFlags.TriggerAllowNextTurn();
    }
}
