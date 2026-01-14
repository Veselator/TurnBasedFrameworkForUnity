public abstract class BingoWinRule : RuleToWinOrDefeat
{
    protected BingoMap _map;
    protected Piece[][] _matrix;

    public override void Init()
    {
        _map = BingoMap.Instance as BingoMap;
    }

    public override RuleWinResult CheckIsPlayerWon(int playerId, TBS_Context context = null)
    {
        BingoContext bContext = context as BingoContext;
        Piece targetPiece; // = bContext == null || bContext.TargetPiece == null ? _map.LastModifiedThing as Piece : bContext.TargetPiece;
        if (bContext == null || bContext.TargetPiece == null)
        {
            targetPiece = _map.LastModifiedThing as Piece;
        }
        else
        {
            targetPiece = bContext.TargetPiece;
        }

        return Check(playerId, targetPiece, bContext);
    }

    protected int GetPieceOwner(int x, int y, BingoContext context)
    {
        // Проверка владельца с учётом контекстаа
        // Контекст в приоритете

        if (context != null)
        {
            Piece contextPiece = context.GetPiece(x, y);
            if (contextPiece != null) return contextPiece.playerId;
        }

        Piece mapPiece = GetPiece(x, y);
        return mapPiece?.playerId ?? -1;
    }

    protected Piece GetPiece(int x, int y)
    {
        if (!IsInBounds(x, y)) return null;
        if (_matrix[y] == null) return null;
        return _matrix[y][x];
    }

    protected bool IsInBounds(int x, int y)
    {
        return x >= 0 && x < _map.Width && y >= 0 && y < _map.Height;
    }

    protected abstract RuleWinResult Check(int playerId, Piece targetPiece, BingoContext context = null);
}
