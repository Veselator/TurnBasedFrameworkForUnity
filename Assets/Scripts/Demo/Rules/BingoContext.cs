using System.Collections;
using System.Collections.Generic;

public class BingoContext : TBS_Context
{
    // Класс для описания контекста в демке бинго

    private Dictionary<(int, int), Piece> _cashedContext;
    public override IEnumerable Context => _cashedContext;

    // То, относительно которой делаем расчёты
    public Piece TargetPiece;

    public BingoContext()
    {
        _cashedContext = new Dictionary<(int, int), Piece>();
    }

    public BingoContext(Piece p) : this()
    {
        _cashedContext[(p.X, p.Y)] = p;
    }

    public BingoContext(Piece p, Piece targetPiece) : this(p)
    {
        TargetPiece = targetPiece;
    }

    public void AddPiece(int x, int y, Piece p)
    {
        _cashedContext[(x, y)] = p;
    }

    public void AddPiece(Piece p)
    {
        AddPiece(p.X, p.Y, p);
    }

    public Piece GetPiece(int x, int y)
    {
        if(_cashedContext.ContainsKey((x, y))) return _cashedContext[(x, y)];
        return null;
    }

    public bool IsPieceMatchesPlayerId(int x, int y, int playerId)
    {
        Piece piece = GetPiece(x, y);
        if(piece == null) return false;
        return playerId == piece.playerId;
    }

    public bool DoesPieceExist(int x, int y)
    {
        return GetPiece(x, y) != null;
    }
}
