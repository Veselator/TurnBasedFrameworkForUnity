using UnityEngine;

[CreateAssetMenu(fileName = "RuleDraftIfMapIsFilled", menuName = "Demo Bingo/RuleDraftIfMapIsFilled")]
public class RuleDraftIfMapIsFilled : BingoWinRule
{
    protected override RuleWinResult Check(int playerId, Piece _, BingoContext context = null)
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        bool isFilled = context == null ? _map.IsEntireMapFilled() : _map.IsEntireMapFilled(context);

        if (isFilled) return new RuleWinResult(GameWinCheckResult.Draft);

        return new RuleWinResult();
    }
}
