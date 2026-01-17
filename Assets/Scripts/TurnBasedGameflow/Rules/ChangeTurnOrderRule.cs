using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeTurnOrderRule", menuName = "TurnBasedGameflow/Rules/ChangeTurnOrderRule")]
public class ChangeTurnOrderRule : RuleAfterEndOfRound
{
    public override IEnumerator ExecuteRule(int roundId, RuleWinResult result)
    {
        // “от, кто победил, не может ходить первым
        if (TBS_OrderManager.Instance.CurrentPlayerID == result.WinnerPlayerID) TBS_OrderManager.Instance.ReverseOrder();
        yield break;
    }
}
