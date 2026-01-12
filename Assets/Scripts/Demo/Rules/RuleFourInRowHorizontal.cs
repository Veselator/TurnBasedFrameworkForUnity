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

    public override RuleWinResult CheckIsPlayerWon(int playerId, TBS_Context context = null)
    {
        if (_map.TotalNumOfElements < 4) return new RuleWinResult();

        BingoContext bContext = context as BingoContext;

        // Берём последнюю изменённую фишку
        Piece targetPiece = bContext == null || bContext.TargetPiece == null ? bContext.TargetPiece : _map.LastModifiedThing as Piece;
        Piece[][] map = _map.Map as Piece[][];

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
            Piece p = map[targetPiece.Y][x];
            bool isMatch = bContext == null ? p.playerId == playerId : p.playerId == playerId || bContext.IsPieceMatchesPlayerId(x, targetPiece.Y, playerId);
            if (isMatch)
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
