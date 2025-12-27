using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TBS_BasePlayerFactory : MonoBehaviour
{
    public static TBS_BasePlayerFactory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public virtual IPlayer CreatePlayer(PlayerInfo info, int id)
    {
        if (info.isAI) return new AIPlayer(id, info.playerName);
        else return new HumanPlayer(id, info.playerName);
    }
}
