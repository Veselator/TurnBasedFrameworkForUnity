using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardBingoAi : MiddleBingoAi
{
    public override void Act()
    {
        List<int> availableColumns = _map.GetAvailableColumns();
        int selectedColumnId = GetColumnIdToBlockOrToWin(availableColumns);

        if (selectedColumnId != -1) {
            Put(selectedColumnId);
            return;
        }

        // ѕытаемс€ зан€ть высоту, с приоритетом в центре
        int mapWidth = _map.Width;
        int toppestY = _map.GetToppestHeight();

        // сортируем доступные колонки по удалЄнности от центра
        availableColumns.Sort((a, b) => 
            Mathf.Abs(a - mapWidth / 2).CompareTo(Mathf.Abs(b - mapWidth / 2))
        );

        foreach (int i in availableColumns)
        {
            if (_map.GetLengthOfColumn(i) < toppestY)
            {
                Put(i);
                return;
            }
        }
        
        Put(availableColumns[0]);
    }

    public HardBingoAi() : base() { }
    public HardBingoAi(int ID, string Name) : base(ID, Name) { }
    public HardBingoAi(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
