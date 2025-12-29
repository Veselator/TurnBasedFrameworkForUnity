using UnityEngine;

[CreateAssetMenu(fileName = "BingoConfig", menuName = "Demo Bingo/BingoConfig")]
public class BingoInitConfig : TBS_InitConfigSO
{
    [Header("Настройки размеров карты бинго")]
    public int mapWidth;
    public int mapHeight;

    // Как вариант, добавить информацию о внешнем виде фишек для гибкой настройки
}