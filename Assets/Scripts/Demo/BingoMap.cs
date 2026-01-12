using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class BingoMap : TBS_BaseMap
{
    // Модель данных карты демки - Бинго
    private PieceColumn[] _map;

    private int _maxHeight;
    public int Height => _maxHeight;

    private int _width;
    public int Width => _width;
    public int TotalNumOfElements { get; private set; } = 0;

    private Piece[][] _outMapCashed;
    private bool _isMapFlagDirty; // Флаг на пересчёт карты
    public override IEnumerable Map => GetMap();

    private Piece lastModifiedThing;
    public override object LastModifiedThing => lastModifiedThing;

    public event Action<int, int, int> OnElementAdded; // x, y, playerId
    public event Action OnMapCreated;
    public event Action OnMapReset;

    public override void Init(TBS_InitConfigSO config)
    {
        BingoInitConfig bingoConfig = config as BingoInitConfig;

        _maxHeight = bingoConfig.mapHeight;
        _width = bingoConfig.mapWidth;
        _isMapFlagDirty = true;

        CreateMap();
    }

    private void CreateMap()
    {
        _map = new PieceColumn[_width];
        for (int i = 0; i < _width; i++)
        {
            _map[i] = new PieceColumn(_maxHeight, i);
        }

        _outMapCashed = new Piece[_maxHeight][];
        OnMapCreated?.Invoke();
    }

    public override void Reload()
    {
        foreach (PieceColumn q in _map)
        {
            q.Clear();
        }

        TotalNumOfElements = 0;
        OnMapReset?.Invoke();
    }

    public bool AddPiece(Piece piece, int columnId)
    {
        if (columnId < 0 || columnId >= _map.Length) return false;
        if (_map[columnId].AddElement(piece))
        {
            // На этом этапе всё нормально
            //UnityEngine.Debug.Log($"Added piece y={piece.Y} x={piece.X} playerId={piece.playerId}");
            lastModifiedThing = piece;
            _isMapFlagDirty = true;
            TotalNumOfElements++;
            OnElementAdded?.Invoke(piece.X, piece.Y, piece.playerId);
            return true;
        }
        return false;
    }

    public bool AddPiece(int playerId, int columnId)
    {
        // Вопрос - почему-бы сразу тут не проверять через NumOfElementsIn
        // влазит ли кусок или нет?
        // Ответ: можно, но разница не существенная

        return AddPiece(new Piece(playerId, columnId, _map[columnId].NumOfElementsIn), columnId);
    }

    public bool IsEntireMapFilled()
    {
        foreach(PieceColumn c in _map)
        {
            if (!c.IsFilled()) return false;
        }

        return true;
    }

    public bool IsEntireMapFilled(BingoContext context)
    {
        foreach (PieceColumn c in _map)
        {
            if (!c.IsFilled())
            {
                // Если в оригинальной карте НЕ заполнена - смотрим, есть ли в контексте
                // Фишка сверху
                // В контексте нет фишки = вся карта не заполнена

                int x = c.ID;
                if (!context.DoesPieceExist(x, c.Length - 1)) return false;
            }
        }

        return true;
    }

    public bool IsColumnFilled(int columnId)
    {
        if (columnId < 0 || columnId >= _map.Length) return false;
        return _map[columnId].IsFilled();
    }

    public List<int> GetAvailableColumns()
    {
        return _map.Where(x => !x.IsFilled()).Select(x => x.ID).ToList();
    }

    public int GetLengthOfColumn(int id)
    {
        if (id < 0 || id >= _map.Length) return 0;
        return _map[id].Length;
    }

    public Piece[][] GetMap()
    {
        // На выходе должна условно такая матрица

        // [4] = 
        // [3] =    P
        // [2] = P  P 
        // [1] = PP P
        // [0] = PP P
        //       0123

        // Где P - Piece

        // ПРОВЕРИТЬ

        if (!_isMapFlagDirty) return _outMapCashed;
        _isMapFlagDirty = false;

        for (int y = 0; y < _maxHeight; y++)
        {
            // Проходимся по каждому столбцу
            // i - номер столбца
            Piece[] newLine = new Piece[_width];

            for (int x = 0; x < _width; x++)
            {
                newLine[x] = _map[x][y];
            }
            //UnityEngine.Debug.Log($"Line y={y} created");
            _outMapCashed[y] = newLine;
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

        return _outMapCashed;
    }
}

public class PieceColumn
{
    // Модель данных столбца
    // Реализует ограничение по высоте

    private Queue<Piece> _column;
    private List<Piece> _columnListCashed;
    private int _maxHeight;
    private const int _defaultHeight = 6;
    public int ID { get; private set; }
    public int Length => _column.Count;

    public Piece this[int id] => GetElement(id);

    public PieceColumn() : this(new Queue<Piece>(), _defaultHeight, 0) { }

    public PieceColumn(int height) : this(new Queue<Piece>(), height, 0) { }
    public PieceColumn(int height, int id) : this(new Queue<Piece>(), height, id) { }

    public PieceColumn(Queue<Piece> column) : this(column, _defaultHeight, 0) { }

    public PieceColumn(Queue<Piece> c, int height, int id)
    {
        _column = c;
        _maxHeight = height;
        _columnListCashed = c.ToList();
        ID = id;
    }

    public int NumOfElementsIn => _column.Count;

    public Piece GetElement(int id)
    {
        if (id < 0 || id >= _column.Count) return null;
        return _columnListCashed[id];
    }

    public void Clear()
    {
        _column.Clear();
        _columnListCashed.Clear();
    }

    public bool AddElement(Piece piece)
    {
        if (_column.Count < _maxHeight)
        {
            _column.Enqueue(piece);
            _columnListCashed.Add(piece);
            return true;
        }
        return false;
    }

    public bool IsFilled()
    {
        return _column.Count >= _maxHeight;
    }
}

public class Piece
{
    // Модель данных отдельного кусочка
    public int playerId { get; private set; }
    public int X;
    public int Y;

    public Piece() : this(-1, 0, 0) { }

    public Piece(int playerId) : this(playerId, 0, 0) { }

    public Piece(int playerId, int x, int y)
    {
        this.playerId = playerId;
        X = x;
        Y = y;
    }

    public static bool operator ==(Piece first, Piece another)
    {
        return first.Equals(another);
    }

    public static bool operator !=(Piece first, Piece another)
    {
        return !first.Equals(another);
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