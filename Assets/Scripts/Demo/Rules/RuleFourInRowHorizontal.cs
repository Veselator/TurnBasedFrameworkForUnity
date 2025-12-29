using System;
using System.Collections;
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

        int maxPiecesInRow = 0;
        int currentPiecesInRow = 0;
        int currentPlayerId = lastPiece.playerId;

        for (int x = startX; x <= endX; x++)
        {
            if (map[lastPiece.Y][x].playerId == currentPlayerId)
            {
                // Если совпадают id владельцев фишек - значит, хорошо
                // Можем продолжать цепь
                currentPiecesInRow++;
            }
            else
            {
                // Если id не совпадают - плохо, цепь разорвана
                if(currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;
                currentPiecesInRow = 0;
            }
        }

        if (currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;

        if (maxPiecesInRow >= 4) return new RuleWinResult() { isWin = true, winnerPlayerID = currentPlayerId };
        else return new RuleWinResult();
    }

    public override IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield break;
    }
}
