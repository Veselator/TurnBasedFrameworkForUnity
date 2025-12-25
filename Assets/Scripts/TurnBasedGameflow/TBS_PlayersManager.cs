using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TBS_PlayersManager : MonoBehaviour
{
    // Менеджер игроков пошаговой системы
    // Отвечает за хранение информации об игроках

    private List<IPlayer> players;
    public IReadOnlyCollection<IPlayer> Players => players;
    public static TBS_PlayersManager Instance { get; private set; }
    private GlobalFlags _globalFlags;
    public int CurrentPlayerID { get; private set; }
    public IPlayer CurrentPlayer => players[CurrentPlayerID];

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
}
