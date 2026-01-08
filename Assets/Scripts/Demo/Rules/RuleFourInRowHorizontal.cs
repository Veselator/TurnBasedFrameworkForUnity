using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFourInRowHorizontal", menuName = "Demo Bingo/RuleFourInRowHorizontal")]
public class RuleFourInRowHorizontal : RuleToWinOrDefeat
{
    // Условие победы в бинго если 4 в ряд горизонтально
    private BingoMap _map;

    public override void Init()
    {
        _map = BingoMap.Instance as BingoMap;
    }

    public override RuleWinResult CheckIsAnybodyWon()
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        // Берём последнюю изменённую фишку
        Piece lastPiece = _map.LastModifiedThing as Piece;
        Piece[][] map = _map.Map as Piece[][];

        // Мы должны проверить такую область
        //
        // -------
        // ***P***
        // -------
        // где P - стартовая фишка
        // * - область проверки

        int startX = Math.Clamp(lastPiece.X - 3, 0, _map.Width - 1);
        int endX = Math.Clamp(lastPiece.X + 3, 0, _map.Width - 1);

        int currentPiecesInRow = 0;
        int currentPlayerId = lastPiece.playerId;

        List<Piece> pieces = new List<Piece>();

        for (int x = startX; x <= endX; x++)
        {
            Piece p = map[lastPiece.Y][x];
            if (p.playerId == currentPlayerId)
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
                    return new BingoWinResult(GameWinCheckResult.Win, currentPlayerId, pieces);
                }

                pieces.Clear();
                currentPiecesInRow = 0;
            }
        }

        if (currentPiecesInRow >= 4)
        {
            return new BingoWinResult(GameWinCheckResult.Win, currentPlayerId, pieces);
        }

        return new RuleWinResult();
    }
}
