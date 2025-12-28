using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BingoMap : TBS_BaseMap
{
    // Модель данных карты демки - Бинго
    private PieceColumn[] _map; // !
    public override IEnumerable Map => throw new System.NotImplementedException();

    public override void Init(TBS_InitConfigSO config)
    {
        _map = new PieceColumn[4];
    }

    public override void Reload()
    {
        foreach (PieceColumn q in _map)
        {
            q.Clear();
        }
    }

    public void AddPiece(Piece piece, int lineId)
    {

    }

    public Piece[][] GetMap(int dgdg)
    {
        return new Piece[dgdg][];
    }
}

public class PieceColumn
{
    private Queue<Piece> column;

    public Piece this[int id]
    {
        get => GetElement(id);
    }

    public PieceColumn()
    {
        column = new Queue<Piece>();
    }

    public PieceColumn(Queue<Piece> column)
    {
        this.column = column;
    }

    public Piece GetElement(int id)
    {
        if (id < 0 || id >= column.Count) return null;
        return column.ToArray()[id];
    }

    public Piece[] GetArray()
    {
        return column.ToArray();
    }

    public void Clear()
    {
        column.Clear();
    }

    public void AddElement(Piece piece)
    {
        column.Enqueue(piece);
    }
}

public class Piece
{
    public int playerId = 0;

    public Piece()
    {
        playerId = 0;
    }
    public Piece(int playerId)
    {
        this.playerId = playerId;
    }
}