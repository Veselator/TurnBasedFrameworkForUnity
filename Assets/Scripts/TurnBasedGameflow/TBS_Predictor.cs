using System.Collections;
using UnityEngine;

public sealed class TBS_Predictor : MonoBehaviour
{
    // Отвечает за предсказание результа победы либо подсчёта очков
    public static TBS_Predictor Instance { get; private set; }
    private TBS_RulesManager _rulesManager;
    private TBS_BaseMap _map;

    private void Awake()
    {
        Instance = this;
    }

    public void Init()
    {
        _map = TBS_BaseMap.Instance;
        _rulesManager = TBS_RulesManager.Instance;
    }

    public RuleWinResult PredictWin(int playerId, TBS_Context context)
    {
        var rulesOfWinOrLose = _rulesManager.RulesToWinOrDefeatCashed;

        if (rulesOfWinOrLose.Count != 0)
        {
            foreach (RuleToWinOrDefeat rule in rulesOfWinOrLose)
            {
                RuleWinResult result = rule.CheckIsPlayerWon(playerId, context);
                if (result.Result == GameWinCheckResult.Win || result.Result == GameWinCheckResult.Draft)
                {
                    // Есть победитель ИЛИ ничья
                    return result;
                }
            }
        }

        return new RuleWinResult();
    }

    public float PredictPoints(int playerId, TBS_Context context)
    {
        return 0f;
    }
}

public abstract class TBS_Context
{
    // Класс контекста для расчёта баллов или определения победителя
    // Нужен для того, что-бы ввести дополнительный контекст без изменения карты

    public abstract IEnumerable Context { get; }
}