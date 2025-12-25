using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerFactory
{
    public static IPlayer CreatePlayer(PlayerInfo info, int id)
    {
        if (info.isAI) return new AIPlayer(id, info.playerName);
        else return new HumanPlayer(id, info.playerName);
    }
}
