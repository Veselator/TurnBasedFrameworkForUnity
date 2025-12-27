using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_BeforeTurnStartHandler : MonoBehaviour
{
    // Обработка правил перед началом хода

    // Правила RulesBeforeTurnCashed

    public static TBS_BeforeTurnStartHandler Instance { get; private set; }
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

        _globalFlags.OnTurnStartedPrepared.AddListener(HandleBeforeTurnStart);
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            _globalFlags.OnTurnStartedPrepared.RemoveListener(HandleBeforeTurnStart);
        }
    }

    public void HandleBeforeTurnStart(int turnId, int playerId)
    {
        List<RuleBeforeTurn> rules = _rulesManager.RulesBeforeTurnCashed;
        if (rules.Count == 0)
        {
            // Правил нет - и нет смысла что-то обрабатывать
            _globalFlags.TriggerOnTurnStarted(turnId, playerId);
            return;
        }
        
        StartCoroutine(ProcessBeforeTurnStartRules(turnId, playerId, rules));
    }

    // Асинхронное выполнение правил перед началом хода
    private IEnumerator ProcessBeforeTurnStartRules(int turnId, int playerId, List<RuleBeforeTurn> rules)
    {
        foreach (IRule rule in rules)
        {
            yield return StartCoroutine(rule.ExecuteRule(turnId, playerId));
        }

        _globalFlags.TriggerOnTurnStarted(turnId, playerId);
    }
}
