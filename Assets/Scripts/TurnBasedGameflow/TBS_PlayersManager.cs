using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class TBS_PlayersManager : MonoBehaviour
{
    // Менеджер игроков пошаговой системы
    // Отвечает за хранение информации об игроках
    // НЕ ЗНАЕТ, КАКОЙ ТЕКУЩИЙ ИГРОК!

    private List<IPlayer> _players;
    public IReadOnlyCollection<IPlayer> Players => _players;
    public static TBS_PlayersManager Instance { get; private set; }
    private GlobalFlags _globalFlags;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags, TBS_InitConfigSO config)
    {
        _globalFlags = globalFlags;
        _players = new List<IPlayer>();

        for (int i = 0; i < config.players.Length; i++)
        {
            _players.Add(PlayerFactory.CreatePlayer(config.players[i], i)); // TODO: переписать PlayerFactory через DI
        }
    }

    public IPlayer GetPlayerByID(int id)
    {
        if(id < 0 || id >= _players.Count) return null;
        return _players[id];
    }

    public int GetPlayersCount()
    {
        return _players.Count;
    }

    public IPlayer GetNextPlayer(int id)
    {
        int nextId = (id + 1) % _players.Count;
        return _players[nextId]; 
    }

    public void AddOverallScoreToPlayerWithId(int who, int howMuch)
    {
        if (who < 0 || who >= _players.Count) return;
        _players[who].OverallScore += howMuch;
    }
    
    public bool IsPlayerAi(int playerId)
    {
        if (playerId < 0 || playerId >= _players.Count) return true;
        return _players[playerId].IsAI;
    }

    public void ResetPlayersPoints()
    {
        foreach(var player in _players)
        {
            player.Points = 0;
        }
    }

    public void ResetPlayersOverallScore()
    {
        foreach (var player in _players)
        {
            player.OverallScore = 0;
        }
    }

    // Название непонянтное
    // Короче, возвращаем игроков в порядке, кто победил
    // 0 - победитель
    // 1 - второе место
    // и тд
    public List<IPlayer> GetPlayersOut()
    {
        return _players.OrderByDescending(p => p.OverallScore).ToList();
    }
}
