using System.Collections.Generic;

public class BingoWinResult : RuleWinResult
{
    public List<Piece> WinPieces;

    public BingoWinResult() : base(GameWinCheckResult.None, -1) { }

    public BingoWinResult(GameWinCheckResult res) : base(res, -1) { }

    public BingoWinResult(GameWinCheckResult res, int playerId, List<Piece> pieces) : base(res, playerId)
    {
        WinPieces = pieces;
    }
}
