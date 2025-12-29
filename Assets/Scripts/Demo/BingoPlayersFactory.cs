public class BingoPlayersFactory : TBS_BasePlayerFactory
{
    // Фабрика игроков для демки бинго
    public override IPlayer CreatePlayer(PlayerInfo info, int id)
    {
        if (info.isAI)
        {
            switch (info.aiDifficulty)
            {
                case 0:
                default:
                    return new EasyBingoAi(id, info.playerName);
                case 1:
                    return new MiddleBingoAi(id, info.playerName);
                case 2:
                    return new HardBingoAi(id, info.playerName);
            }
        }
        else return new HumanPlayer(id, info.playerName);
    }
}