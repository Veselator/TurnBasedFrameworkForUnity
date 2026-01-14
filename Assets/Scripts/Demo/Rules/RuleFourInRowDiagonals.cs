using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFourInRowDiagonals", menuName = "Demo Bingo/RuleFourInRowDiagonals")]
public class RuleFourInRowDiagonals : BingoWinRule
{
    protected override RuleWinResult Check(int playerId, Piece targetPiece, BingoContext context = null)
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        _matrix = _map.Map as Piece[][];

        List<Piece> pieces = CheckDiagonal(targetPiece.X, targetPiece.Y, playerId, 1, 1, context);

        if (pieces != null) return new BingoWinResult(GameWinCheckResult.Win, playerId, pieces);

        pieces = CheckDiagonal(targetPiece.X, targetPiece.Y, playerId, 1, -1, context);
        if (pieces != null) return new BingoWinResult(GameWinCheckResult.Win, playerId, pieces);

        return new RuleWinResult();
    }

    private List<Piece> CheckDiagonal(int pieceX, int pieceY, int playerId, int dirX, int dirY, BingoContext context)
    {
        int startX = pieceX;
        int startY = pieceY;

        List<Piece> pieces = new List<Piece>();

        for (int i = 0; i < 3; i++)
        {
            int prevX = startX - dirX;
            int prevY = startY - dirY;

            if (!IsInBounds(prevX, prevY))
                break;

            startX = prevX;
            startY = prevY;
        }

        int currentInRow = 0;

        int x = startX;
        int y = startY;

        while (IsInBounds(x, y))
        {
            Piece piece = GetPiece(x, y);
            int pieceOwner = GetPieceOwner(x, y, context);

            if (pieceOwner == playerId)
            {
                pieces.Add(piece);
                currentInRow++;
            }
            else
            {
                // Если уже нашли - нет смысла дальше искать
                if (currentInRow >= 4)
                {
                    return pieces;
                }

                pieces.Clear();
                currentInRow = 0;
            }

            x += dirX;
            y += dirY;

            // Защита от бесконечного цикла
            if (Math.Abs(x - startX) > 10) break;
        }

        if (currentInRow >= 4)
        {
            return pieces;
        }

        return null;
    }
}