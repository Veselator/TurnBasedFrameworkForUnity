using UnityEngine;

public class BingoVisualMapGenerator : MonoBehaviour
{
    // Отвечает за корректное создание карты Бинго
    [SerializeField] private GameObject _mapPrefab;
    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _distanceX = 0.4f;
    [SerializeField] private float _distanceY = 1.0f;
    [SerializeField] private GameObject _point;

    public void Init()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        // Непосредственная генерация карты
        BingoMap bingoMap = TBS_BaseMap.Instance as BingoMap;
        Piece[][] mapMatrix = bingoMap.Map as Piece[][];

        int width = bingoMap.Width;
        int height = bingoMap.Height;

        // Центрируем startPoint по оси X относительно нулевых координат
        // Чтобы _startPoint соответствовал нижнему левому углу поля,
        // а само поле было отцентрировано по X = 0
        CenterStartPoint(width);

        Transform[][] points = new Transform[height][];

        float startX = _startPoint.position.x;
        float startY = _startPoint.position.y;

        GameObject map = Instantiate(_mapPrefab);

        for (int y = height - 1; y >= 0; y--)
        {
            Transform[] line = new Transform[width];
            for (int x = 0; x < width; x++)
            {
                Vector3 newPosition = new Vector3(startX + x * _distanceX, startY + y * _distanceY, 0);
                GameObject point = Instantiate(_point, newPosition, Quaternion.identity, map.transform);
                line[x] = point.transform;
            }
            points[y] = line;
        }

        map.GetComponent<BingoVisualMap>().Init(points, TBS_BaseMap.Instance as BingoMap, _distanceX, _distanceY);
    }

    private void CenterStartPoint(int width)
    {
        float totalWidth = (width - 1) * _distanceX;

        float centeredStartX = -totalWidth / 2f;

        _startPoint.position = new Vector3(
            centeredStartX,
            _startPoint.position.y,
            _startPoint.position.z
        );
    }
}