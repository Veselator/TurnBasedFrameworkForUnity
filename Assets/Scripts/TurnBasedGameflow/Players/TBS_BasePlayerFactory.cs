using UnityEngine;

public class TBS_BasePlayerFactory : MonoBehaviour
{
    // Фабрика игроков пошаговой игры
    // Для адаптации под конкретную систему создаём класс, который наследуется от TBS_BasePlayerFactory
    // И заменяем TBS_BasePlayerFactory на этот компонент в префабе TBS
    // И всё - теперь у нас другая фабрика, которая создаёт нужных игроков
    // ПРИ ЭТОМ не надо переписывать классы высокого уровня - скажем, TBS_PlayersManager
    // Там используется общий интерфейс IPlayer

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
