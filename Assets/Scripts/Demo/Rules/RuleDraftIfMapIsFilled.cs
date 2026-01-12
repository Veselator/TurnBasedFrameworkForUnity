using UnityEngine;

[CreateAssetMenu(fileName = "RuleDraftIfMapIsFilled", menuName = "Demo Bingo/RuleDraftIfMapIsFilled")]
public class RuleDraftIfMapIsFilled : RuleToWinOrDefeat
{
    private BingoMap _map;

    public override void Init()
    {
        _map = BingoMap.Instance as BingoMap;
    }

    public override RuleWinResult CheckIsPlayerWon(int playerId, TBS_Context context = null)
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        bool isFilled = context == null ? _map.IsEntireMapFilled() : _map.IsEntireMapFilled(context as BingoContext);

        if (isFilled) return new RuleWinResult(GameWinCheckResult.Draft);

        return new RuleWinResult();
    }
}
