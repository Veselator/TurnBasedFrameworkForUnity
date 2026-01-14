using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFourInRowHorizontal", menuName = "Demo Bingo/RuleFourInRowHorizontal")]
public class RuleFourInRowHorizontal : BingoWinRule
{
    protected override RuleWinResult Check(int playerId, Piece targetPiece, BingoContext context = null)
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();
        _matrix = _map.Map as Piece[][];

        // Мы должны проверить такую область
        //
        // -------
        // ***P***
        // -------
        // где P - стартовая фишка
        // * - область проверки

        int startX = Math.Clamp(targetPiece.X - 3, 0, _map.Width - 1);
        int endX = Math.Clamp(targetPiece.X + 3, 0, _map.Width - 1);

        int currentPiecesInRow = 0;

        List<Piece> pieces = new List<Piece>();

        for (int x = startX; x <= endX; x++)
        {
            Piece p = _matrix[targetPiece.Y][x];
            if (playerId == GetPieceOwner(x, targetPiece.Y, context))
            {
                // Если совпадают id владельцев фишек - значит, хорошо
                // Можем продолжать цепь
                //Debug.Log($"HORIZONTAL Piece at y={lastPiece.Y} x={x} matches currentPlayerId {currentPlayerId}; In row {currentPiecesInRow} pieces; max pieces {maxPiecesInRow}");
                currentPiecesInRow++;
                pieces.Add(p);
            }
            else
            {
                // Если id не совпадают - плохо, цепь разорвана
                if (currentPiecesInRow >= 4)
                {
                    return new BingoWinResult(GameWinCheckResult.Win, playerId, pieces);
                }

                pieces.Clear();
                currentPiecesInRow = 0;
            }
        }

        if (currentPiecesInRow >= 4)
        {
            return new BingoWinResult(GameWinCheckResult.Win, playerId, pieces);
        }

        return new RuleWinResult();
    }
}
