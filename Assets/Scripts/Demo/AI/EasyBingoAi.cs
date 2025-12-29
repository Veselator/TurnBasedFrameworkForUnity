using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EasyBingoAi : BingoAi
{
    public override void Act()
    {
        List<int> availableColumns = _map.GetAvailableColumns();
        int selectedColumnId = availableColumns[Random.Range(0, availableColumns.Count)];

        _map.AddPiece(ID, selectedColumnId);
        _globalFlags.TriggerOnTurnEnded(_turnsManager.CurrentTurn, ID);
    }

    public EasyBingoAi() : base() { }
    public EasyBingoAi(int ID, string Name) : base(ID, Name) { }
    public EasyBingoAi(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
