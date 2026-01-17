using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleWaitSomeTimeAfterRound", menuName = "TurnBasedGameflow/Rules/RuleWaitSomeTimeAfterRound")]
public class WaitSomeTimeAfterRound : RuleAfterEndOfRound
{
    public float timeToWait;
    public override IEnumerator ExecuteRule(int turnId, RuleWinResult _)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}