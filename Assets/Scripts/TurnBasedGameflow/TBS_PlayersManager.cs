using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TBS_PlayersManager : MonoBehaviour
{
    // Менеджер игроков пошаговой системы
    // Отвечает за хранение информации об игроках
    // НЕ ЗНАЕТ, КАКОЙ ТЕКУЩИЙ ИГРОК!

    private List<IPlayer> players;
    public IReadOnlyCollection<IPlayer> Players => players;
    public static TBS_PlayersManager Instance { get; private set; }
    private GlobalFlags _globalFlags;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags, TBS_InitConfigSO config)
    {
        _globalFlags = globalFlags;
        players = new List<IPlayer>();

        for (int i = 0; i < config.players.Length; i++)
        {
            players.Add(PlayerFactory.CreatePlayer(config.players[i], i)); // TODO: переписать PlayerFactory через DI
        }
    }

    public IPlayer GetPlayerByID(int id)
    {
        if(id < 0 || id >= players.Count) return null;
        return players[id];
    }

    public int GetPlayersCount()
    {
        return players.Count;
    }

    public IPlayer GetNextPlayer(int id)
    {
        int nextId = (id + 1) % players.Count;
        return players[nextId]; 
    }
}
