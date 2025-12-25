using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                break;
            }
        }

        foreach (var player in players)
        {
            if (player.IsAI)
            {
                order.Add(player.ID);
            }
        }
        return order;
    }
}
