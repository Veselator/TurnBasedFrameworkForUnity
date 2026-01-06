using System.Collections;
using UnityEngine;

// ѕравило ожидани€ N времени после конца хода

[CreateAssetMenu(fileName = "RuleWaitSomeTimeAfterTurn", menuName = "TurnBasedGameflow/Rules/RuleWaitSomeTimeAfterTurn")]
public class WaitSomeTimeAfterTurn : RuleBeforeTurnEnd
{
    public float timeToWait;
    public override IEnumerator ExecuteRule()
    {
        yield return new WaitForSeconds(timeToWait);
    }

    public override IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield return ExecuteRule();
    }
}