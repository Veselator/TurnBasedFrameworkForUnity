using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "RuleFourInRowDiagonals", menuName = "Demo Bingo/RuleFourInRowDiagonals")]
public class RuleFourInRowDiagonals : RuleToWinOrDefeat
{
    // Условие победы в бинго если 4 в ряд по диагоналям
    private BingoMap _map;
    private Piece[][] _matrix;

    public override void Init()
    {
        _map = BingoMap.Instance as BingoMap;
        // Так как в классе карты матрица не пересоздаётся (значения линий просто перезаписываются),
        // _matrix будет ссылаться на актуальную карту
        //_matrix = _map.Map as Piece[][];
    }

    public override RuleWinResult CheckIsAnybodyWon()
    {
        // Небольшая оптимизация - если фишек меньше 4, то никто в принципе не может победить
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        if(_matrix == null) _matrix = _map.Map as Piece[][];

        // Берём последнюю изменённую фишку
        Piece lastPiece = _map.LastModifiedThing as Piece;

        // Мы должны проверить такую область
        // *     *
        //  *   *
        //   * *
        //    P
        //   * *
        //  *   *
        // *     *
        // где P - стартовая фишка
        // * - область проверки

        int startX = Math.Min(lastPiece.X - 3,  _map.Width - 1);
        int endX = Math.Min(lastPiece.X + 3, _map.Width - 1);

        int startY = Math.Min(lastPiece.Y - 3, _map.Height - 1);
        int endY = Math.Min(lastPiece.Y + 3, _map.Height - 1);

        int currentPlayerId = lastPiece.playerId;

        int maxPiecesInRow = GetMaxPiecesInRowDiagonal(startX, startY, endX, endY, currentPlayerId, 1, 1);
        if (maxPiecesInRow >= 4) return new RuleWinResult() { isWin = true, winnerPlayerID = currentPlayerId };

        maxPiecesInRow = GetMaxPiecesInRowDiagonal(startX, endY, endX, startY, currentPlayerId, 1, -1);
        if (maxPiecesInRow >= 4) return new RuleWinResult() { isWin = true, winnerPlayerID = currentPlayerId };
        else return new RuleWinResult();
    }

    private int GetMaxPiecesInRowDiagonal(int startX, int startY, int endX, int endY, int currentPlayerId, int directionX, int directionY)
    {
        int maxPiecesInRow = 0;
        int currentPiecesInRow = 0;

        int y = startY;

        // Проблема - работает только пока startX > endX
        // Но в рамках программы startX всегда будет меньше endX
        // Или как вариант, использовать дополнительные переменные и тернарный оператор

        for (int x = startX; x <= endX; x += directionX)
        {
            if (y >= 0 && x >= 0 && y < _map.Height)
            {
                if (_matrix[y] != null && _matrix[y][x].playerId == currentPlayerId)
                {
                    // Если совпадают id владельцев фишек - значит, хорошо
                    // Можем продолжать цепь
                    currentPiecesInRow++;
                }
                else
                {
                    // Если id не совпадают - плохо, цепь разорвана
                    if (currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;
                    currentPiecesInRow = 0;
                }
            }
            y += directionY;
        }

        //while (x <= endX && y <= endY)
        //{
        //    if (y >= 0 && x >= 0)
        //    {
        //        if (_matrix[y][x].playerId == currentPlayerId)
        //        {
        //            // Если совпадают id владельцев фишек - значит, хорошо
        //            // Можем продолжать цепь
        //            currentPiecesInRow++;
        //        }
        //        else
        //        {
        //            // Если id не совпадают - плохо, цепь разорвана
        //            if (currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;
        //            currentPiecesInRow = 0;
        //        }
        //    }
        //    x += directionX;
        //    y += directionY;
        //}
        if (currentPiecesInRow > maxPiecesInRow) maxPiecesInRow = currentPiecesInRow;
        return maxPiecesInRow;
    }

    public override IEnumerator ExecuteRule(int turnId, int playerId)
    {
        yield break;
    }
}
