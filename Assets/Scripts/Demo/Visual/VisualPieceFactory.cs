using UnityEngine;

public class VisualPieceFactory : MonoBehaviour
{
    // Фабрика визуальных фишек для бинго

    private GameObject[] _pieces;
    public static VisualPieceFactory Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Init(TBS_InitConfigSO config)
    {
        BingoInitConfig bingoConfig = config as BingoInitConfig;
        _pieces = bingoConfig.piecePrefabsForEachPlayer;
    }

    public static GameObject GetPiece(int playerId)
    {
        if(Instance == null) return null;

        if (playerId < 0 || playerId >= Instance._pieces.Length) return null;
        return Instance._pieces[playerId];
    }
}
