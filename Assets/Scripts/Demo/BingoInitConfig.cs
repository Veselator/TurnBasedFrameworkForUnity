using UnityEngine;

[CreateAssetMenu(fileName = "Bingo config", menuName = "Demo Bingo/Bingo config")]
public class BingoInitConfig : TBS_InitConfigSO
{
    public int mapWidth;
    public int mapHeight;

    // Как вариант, добавить информацию о внешнем виде фишек для гибкой настройки
}
