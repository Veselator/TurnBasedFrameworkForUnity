using System;
using System.Collections.Generic;
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

        int currentPiecesInRow = 0;
        int currentPlayerId = lastPiece.playerId;

        List<Piece> pieces = new List<Piece>();

        for (int y = startY; y <= endY; y++)
        {
            Piece p = map[y][lastPiece.X];
            if (p.playerId == currentPlayerId)
            {
                // Если совпадают id владельцев фишек - значит, хорошо
                // Можем продолжать цепь
                //Debug.Log($"VERTICAL Piece at y={y} x={lastPiece.X} matches currentPlayerId {currentPlayerId}; In row {currentPiecesInRow} pieces; max pieces {maxPiecesInRow}");
                currentPiecesInRow++;
                pieces.Add(p);
            }
            else
            {
                // Если id не совпадают - цепь разорвана
                // Проверяем, победа ли это
                if (currentPiecesInRow >= 4)
                {
                    return new BingoWinResult(GameWinCheckResult.Win, currentPlayerId, pieces);
                }

                pieces.Clear();
                currentPiecesInRow = 0;
            }
        }

        // Если дошли до сюда - значит, ничего не нашли
        return new RuleWinResult();
    }
}
