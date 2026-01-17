using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitTimeAfterGameEndRule", menuName = "TurnBasedGameflow/Rules/WaitTimeAfterGameEndRule")]
public class WaitTimeAfterGameEnd : RuleBeforeEndOfGame
{
    public float waitTime = 2f;

    public override IEnumerator ExecuteRule(List<IPlayer> players)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
