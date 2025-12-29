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
        BingoMap bingoMap = TBS_BaseMap.Instance as BingoMap;
        Piece[][] mapMatrix = bingoMap.Map as Piece[][];

        int width = bingoMap.Width;
        int height = bingoMap.Height;

        Transform[][] points = new Transform[height][];

        float startX = _startPoint.position.x;
        float startY = _startPoint.position.y;

        GameObject map = Instantiate(_mapPrefab);

        for (int y = height - 1; y >= 0; y--)
        {
            //Debug.Log($"y = {y}");
            Transform[] line = new Transform[width];
            for(int x = 0; x < width; x++)
            {
                Vector3 newPosition = new Vector3(startX + x * _distanceX, startY + y * _distanceY, 0);

                //GameObject point = new GameObject(_point);
                //point.transform.position = newPosition;
                //point.transform.parent = map.transform;
                GameObject point = Instantiate(_point, newPosition, Quaternion.identity, map.transform);

                line[x] = point.transform;
            }

            points[y] = line;
        }

        map.GetComponent<BingoVisualMap>().Init(points, TBS_BaseMap.Instance as BingoMap);
    }
}
