using UnityEngine;

[CreateAssetMenu(fileName = "BingoConfig", menuName = "Demo Bingo/BingoConfig")]
public class BingoInitConfig : TBS_InitConfigSO
{
    [Header("Настройки размеров карты бинго")]
    public int mapWidth;
    public int mapHeight;

    // Передаём информацию о внещнем виде фишек ссылкой на префабы
    public GameObject[] piecePrefabsForEachPlayer;
}