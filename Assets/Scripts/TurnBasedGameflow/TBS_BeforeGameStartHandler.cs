using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_BeforeGameStartHandler : MonoBehaviour
{
    // Обработка правил перед началом игры

    // Правила RulesBeforeTurnCashed

    public static TBS_BeforeGameStartHandler Instance { get; private set; }
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

        _globalFlags.OnGameStartedQuery.AddListener(HandleGameStartQuery);
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            _globalFlags.OnGameStartedQuery.RemoveListener(HandleGameStartQuery);
        }
    }

    public void HandleGameStartQuery()
    {
        List<RuleBeforeGameStart> rules = _rulesManager.RulesBeforeGameStartCashed;
        if (rules.Count == 0)
        {
            // Правил нет - и нет смысла что-то обрабатывать
            _globalFlags.TriggerOnGameStartedAllowed();
            return;
        }

        StartCoroutine(ProcessBeforeGameStartRules(rules));
    }

    // Асинхронное выполнение правил перед началом хода
    private IEnumerator ProcessBeforeGameStartRules(List<RuleBeforeGameStart> rules)
    {
        foreach (IRule rule in rules)
        {
            yield return StartCoroutine(rule.ExecuteRule());
        }

        _globalFlags.TriggerOnGameStartedAllowed();
    }
}
