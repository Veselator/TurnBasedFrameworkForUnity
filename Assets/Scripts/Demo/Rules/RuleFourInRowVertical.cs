using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFourInRowVertical", menuName = "Demo Bingo/RuleFourInRowVertical")]
public class RuleFourInRowVertical : RuleToWinOrDefeat
{
    // Условие победы в бинго если 4 в ряд вертикально
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
        // *
        // *
        // *
        // P
        // *
        // *
        // *
        // где P - стартовая фишка
        // * - область проверки

        int startY = Math.Clamp(lastPiece.Y - 3, 0, _map.Height - 1);
        int endY = Math.Clamp(lastPiece.Y + 3, 0, _map.Height - 1);

        int maxPiecesInRow = 0;
        int currentPiecesInRow = 0;
        int currentPlayerId = lastPiece.playerId;

        for (int y = startY; y <= endY; y++)
        {
            if (map[y][lastPiece.X].playerId == currentPlayerId)
            {
                // Если совпадают id владельцев фишек - значит, хорошо
                // Можем продолжать цепь
                //Debug.Log($"VERTICAL Piece at y={y} x={lastPiece.X} matches currentPlayerId {currentPlayerId}; In row {currentPiecesInRow} pieces; max pieces {maxPiecesInRow}");
                currentPiecesInRow++;
            }
            else
            {
                // Если id не совпадают - плохо, цепь разорвана
                if (currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;
                currentPiecesInRow = 0;
            }
        }

        if (currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;
        //Debug.Log($"Total max pieces vertical {maxPiecesInRow}");

        if (maxPiecesInRow >= 4) return new RuleWinResult() { isWin = true, winnerPlayerID = currentPlayerId };
        else return new RuleWinResult();
    }
}
