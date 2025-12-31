using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TBS_PlayerFirstRule", menuName = "TurnBasedGameflow/TBS_PlayerFirstRule")]
public class TBS_PlayerFirstRule : TBS_BaseOrderRule
{
    public override List<int> GetTurnOrder(IReadOnlyCollection<IPlayer> players)
    {
        List<int> order = new List<int>();

        foreach (var player in players)
        {
            if (!player.IsAI)
            {
                order.Add(player.ID);
            }
        }

        foreach (var player in players)
        {
            if (!order.Contains(player.ID))
            {
                order.Add(player.ID);
            }
        }
        return order;
    }
}
