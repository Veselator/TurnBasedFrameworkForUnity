using System.Collections;
using UnityEngine;

// ѕравило ожидани€ N времени после конца хода

[CreateAssetMenu(fileName = "WaitTimeBeforeGameStarts", menuName = "TurnBasedGameflow/Rules/WaitTimeBeforeGameStarts")]
public class WaitTimeBeforeGameStarts : RuleBeforeGameStart
{
    public float timeToWait;
    public override IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield return ExecuteRule();
    }

    public override IEnumerator ExecuteRule()
    {
        yield return new WaitForSeconds(timeToWait);
    }
}