using System;
using System.Collections;
using System.Collections.Generic;

public class BingoMap : TBS_BaseMap
{
    // Модель данных карты демки - Бинго
    private PieceColumn[] _map;
    private int _maxHeight;
    private int _width;
    public override IEnumerable Map => GetMap();

    public event Action OnElementAdded;
    public event Action OnMapCreated;
    public event Action OnMapReset;

    public override void Init(TBS_InitConfigSO config)
    {
        BingoInitConfig bingoConfig = config as BingoInitConfig;

        _maxHeight = bingoConfig.mapHeight;
        _width = bingoConfig.mapWidth;

        CreateMap();
    }

    private void CreateMap()
    {
        _map = new PieceColumn[_width];
        for (int i = 0; i < _width; i++)
        {
            _map[i] = new PieceColumn(_maxHeight);
        }

        OnMapCreated?.Invoke();
    }

    public override void Reload()
    {
        foreach (PieceColumn q in _map)
        {
            q.Clear();
        }

        OnMapReset?.Invoke();
    }

    public void AddPiece(Piece piece, int columnId)
    {
        if (columnId < 0 || columnId >= _map.Length) return;
        if (_map[columnId].AddElement(piece)) OnElementAdded?.Invoke();
    }

    public Piece[][] GetMap()
    {
        // На выходе должно условно такая матрица

        // [4] = 
        // [3] =    P
        // [2] = P  P 
        // [1] = PP P
        // [0] = PP P
        //       0123

        // Где P - Piece

        Piece[][] outMap = new Piece[_maxHeight][];

        for (int y = 0; y < _maxHeight; y++)
        {
            // Проходимся по каждому столбцу
            // i - номер столбца
            Piece[] newLine = new Piece[_width];

            for (int x = 0; x < _width; x++)
            {
                newLine[x] = _map[x][y];
            }

            outMap[y] = newLine;
        }

        // Пример как надо проходиться по массиву для получения правильного порядка
        // Хотя для, скажем, расчёта победной комбинации порядок обхода не влияет на результат
        //Piece[][] mapMatrix = map.Map as Piece[][];

        //for (int y = mapMatrix.Length - 1; y >= 0; y--)
        //{
        //    foreach (Piece piece in mapMatrix[y])
        //    {
        //        Console.Write(piece.playerId + 1 + " ");
        //    }

        //    Console.WriteLine();
        //}

        return outMap;
    }
}

public class PieceColumn
{
    // Модель данных столбца
    // Реализует ограничение по высоте

    private Queue<Piece> _column;
    private Piece[] _columnArrayCashed;
    private int _maxHeight;
    private const int _defaultHeight = 6;

    public Piece this[int id] => GetElement(id);

    public PieceColumn() : this(new Queue<Piece>(), _defaultHeight) { }

    public PieceColumn(int height) : this(new Queue<Piece>(), height) { }

    public PieceColumn(Queue<Piece> column) : this(column, _defaultHeight) { }

    public PieceColumn(Queue<Piece> c, int height)
    {
        _column = c;
        _maxHeight = height;
        _columnArrayCashed = c.ToArray();
    }

    public Piece GetElement(int id)
    {
        if (id < 0 || id >= _column.Count) return new Piece();
        return _columnArrayCashed[id];
    }

    public Piece[] GetArray()
    {
        return _columnArrayCashed;
    }

    public void Clear()
    {
        _column.Clear();
    }

    public bool AddElement(Piece piece)
    {
        if (_column.Count < _maxHeight)
        {
            _column.Enqueue(piece);
            _columnArrayCashed = _column.ToArray();
            return true;
        }
        return false;
    }
}

public class Piece
{
    // Модель данных отдельного кусочка
    public int playerId;

    public Piece()
    {
        playerId = -1;
    }

    public Piece(int playerId)
    {
        this.playerId = playerId;
    }

    public static bool operator ==(Piece first, Piece another)
    {
        return first.Equals(another);
    }

    public static bool operator !=(Piece first, Piece another)
    {
        return first.Equals(another);
    }

    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        Piece another = obj as Piece;
        if (another == null) return false;
        return playerId == another.playerId;
    }

    public override int GetHashCode()
    {
        return playerId.GetHashCode();
    }
}