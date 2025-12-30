using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleWaitSomeTimeAfterRound", menuName = "TurnBasedGameflow/RuleWaitSomeTimeAfterRound")]
public class WaitSomeTimeAfterRound : RuleAfterEndOfRound
{
    public float timeToWait;
    public override IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}