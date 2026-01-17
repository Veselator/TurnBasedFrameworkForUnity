using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_BeforeGameEndHandler : MonoBehaviour
{
    // Обработка правил после конца игры

    // Правила RulesAfterEndOfGameCashed
    public static TBS_BeforeGameEndHandler Instance { get; private set; }
    private GlobalFlags _globalFlags;
    private TBS_RulesManager _rulesManager;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _rulesManager = TBS_RulesManager.Instance;

        _globalFlags.OnGameEndedQuery.AddListener(HandleBeforeGameEnded);
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            _globalFlags.OnGameEndedQuery.RemoveListener(HandleBeforeGameEnded);
        }
    }

    private void HandleBeforeGameEnded(List<IPlayer> players)
    {
        StartCoroutine(ProcessBeforeGameEndRules(players));
    }

    private IEnumerator ProcessBeforeGameEndRules(List<IPlayer> players)
    {
        var rules = _rulesManager.RulesBeforeEndOfGameCashed;

        if(rules.Count != 0)
        {
            foreach (var rule in rules)
            {
                yield return StartCoroutine(rule.ExecuteRule(players));
            }
        }

        _globalFlags.TriggerOnGameEnded(players);
    }
}