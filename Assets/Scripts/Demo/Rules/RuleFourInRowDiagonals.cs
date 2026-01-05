using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFourInRowDiagonals", menuName = "Demo Bingo/RuleFourInRowDiagonals")]
public class RuleFourInRowDiagonals : RuleToWinOrDefeat
{
    private BingoMap _map;
    private Piece[][] _matrix;

    public override void Init()
    {
        _map = BingoMap.Instance as BingoMap;
    }

    public override RuleWinResult CheckIsAnybodyWon()
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        _matrix = _map.Map as Piece[][];

        Piece lastPiece = _map.LastModifiedThing as Piece;
        int currentPlayerId = lastPiece.playerId;

        int count1 = CheckDiagonal(lastPiece.X, lastPiece.Y, currentPlayerId, 1, 1);
        if (count1 >= 4) return new RuleWinResult() { isWin = true, winnerPlayerID = currentPlayerId };

        int count2 = CheckDiagonal(lastPiece.X, lastPiece.Y, currentPlayerId, 1, -1);
        if (count2 >= 4) return new RuleWinResult() { isWin = true, winnerPlayerID = currentPlayerId };

        return new RuleWinResult();
    }

    private int CheckDiagonal(int pieceX, int pieceY, int playerId, int dirX, int dirY)
    {
        int startX = pieceX;
        int startY = pieceY;

        for (int i = 0; i < 3; i++)
        {
            int prevX = startX - dirX;
            int prevY = startY - dirY;

            if (!IsInBounds(prevX, prevY))
                break;

            startX = prevX;
            startY = prevY;
        }

        int maxInRow = 0;
        int currentInRow = 0;

        int x = startX;
        int y = startY;

        while (IsInBounds(x, y))
        {
            Piece piece = GetPiece(x, y);
            int pieceOwner = piece?.playerId ?? -1;

            if (pieceOwner == playerId)
            {
                currentInRow++;
                if (currentInRow > maxInRow)
                    maxInRow = currentInRow;
            }
            else
            {
                currentInRow = 0;
            }

            x += dirX;
            y += dirY;

            // Защита от бесконечного цикла
            if (Math.Abs(x - startX) > 10) break;
        }

        return maxInRow;
    }

    private bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < _map.Width && y >= 0 && y < _map.Height;
    }

    private Piece GetPiece(int x, int y)
    {
        if (!IsInBounds(x, y)) return null;
        if (_matrix[y] == null) return null;
        return _matrix[y][x];
    }
}