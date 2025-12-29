using System.Collections;
using UnityEngine;

// ѕравило ожидани€ N времени после конца хода

[CreateAssetMenu(fileName = "RuleWaitSomeTimeAfterTurn", menuName = "TurnBasedGameflow/RuleWaitSomeTimeAfterTurn")]
public class WaitSomeTimeAfterTurn : RuleBeforeTurnEnd
{
    public float timeToWait;
    public override IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield return new WaitForSeconds(timeToWait);
    }
}