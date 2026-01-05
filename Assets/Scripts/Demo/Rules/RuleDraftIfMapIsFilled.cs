using UnityEngine;

[CreateAssetMenu(fileName = "RuleDraftIfMapIsFilled", menuName = "Demo Bingo/RuleDraftIfMapIsFilled")]
public class RuleDraftIfMapIsFilled : RuleToWinOrDefeat
{
    private BingoMap _map;

    public override void Init()
    {
        _map = BingoMap.Instance as BingoMap;
    }

    public override RuleWinResult CheckIsAnybodyWon()
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        if(_map.IsEntireMapFilled()) return new RuleWinResult() { isDraft = true };

        return new RuleWinResult();
    }
}
