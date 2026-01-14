using System.Collections.Generic;
using UnityEngine;

public class MiddleBingoAi : BingoAi
{
    private BingoContext context;

    public override void Init(GlobalFlags gf)
    {
        base.Init(gf);
        context = new BingoContext();
    }

    public override void Init(GlobalFlags gf, TBS_TurnsManager t)
    {
        base.Init(gf, t);
        context = new BingoContext();
    }

    public override void Act()
    {
        List<int> availableColumns = _map.GetAvailableColumns();
        int selectedColumnId = GetColumnIdToBlockOrToWin(availableColumns); // availableColumns[Random.Range(0, availableColumns.Count)];

        Put(selectedColumnId == -1 ? availableColumns[Random.Range(0, availableColumns.Count)] : selectedColumnId);
    }

    protected int GetColumnIdToBlockOrToWin(List<int> availableColumns)
    {
        context.Clear();
        context.ClearTargetPiece();

        // Логика среднего ИИ - смотрим, может ли где-то противник выиграть
        // если может - блокируем
        // Потом смотрим, можем ли мы выиграть
        // Можем - выигрываем
        // Иначе - случайный

        // Проблемы:
        // - не думает на два шага вперёд
        // - если возможность победить будет правее, чем возможность заблокировать - он заблокирует
        // нужно проходить отдельно, двумя циклами

        foreach (int columnId in availableColumns)
        {
            int x = columnId;
            int y = _map.GetLengthOfColumn(columnId);

            Piece p = new Piece(ID, x, y);
            context.SetPiece(x, y, p);
            context.SetTargetPiece(p);
            bool willAiWin = _predictor.PredictWin(ID, context).WinnerPlayerID == ID;

            if (willAiWin)
            {
                return columnId;
            }

            context.ClearPiece(x, y);
        }

        context.Clear();

        foreach (int columnId in availableColumns)
        {
            int x = columnId;
            int y = _map.GetLengthOfColumn(columnId);

            Piece p = new Piece(_humanPlayerId, x, y);
            context.SetPiece(x, y, p);
            context.SetTargetPiece(p);
            bool willHumanWin = _predictor.PredictWin(_humanPlayerId, context).WinnerPlayerID == _humanPlayerId;

            if (willHumanWin)
            {
                return columnId;
            }
            context.ClearPiece(x, y);
        }

        return -1; // Не нашли
    }

    public MiddleBingoAi() : base() { }
    public MiddleBingoAi(int ID, string Name) : base(ID, Name) { }
    public MiddleBingoAi(int ID, string Name, int Points) : base(ID, Name, Points) { }
}
