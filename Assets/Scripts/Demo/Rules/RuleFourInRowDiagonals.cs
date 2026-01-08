using System;
using System.Collections.Generic;
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

        List<Piece> pieces = CheckDiagonal(lastPiece.X, lastPiece.Y, currentPlayerId, 1, 1);

        if (pieces != null) return new BingoWinResult(GameWinCheckResult.Win, currentPlayerId, pieces);

        pieces = CheckDiagonal(lastPiece.X, lastPiece.Y, currentPlayerId, 1, -1);
        if (pieces != null) return new BingoWinResult(GameWinCheckResult.Win, currentPlayerId, pieces);

        return new RuleWinResult();
    }

    private List<Piece> CheckDiagonal(int pieceX, int pieceY, int playerId, int dirX, int dirY)
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
            int pieceOwner = piece?.playerId ?? -1;

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