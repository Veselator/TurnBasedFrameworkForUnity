using UnityEngine;

public class BingoVisualMapGenerator : MonoBehaviour
{
    // Отвечает за корректное создание карты
    [SerializeField] private GameObject _mapPrefab;

    [SerializeField] private Transform _startPoint;
    [SerializeField] private float _distanceX = 0.4f;
    [SerializeField] private float _distanceY = 1.0f;

    public void Init()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        Piece[][] mapMatrix = TBS_BaseMap.Instance.Map as Piece[][];
        float startX = _startPoint.position.x;
        float startY = _startPoint.position.y;

        GameObject map = Instantiate(_mapPrefab);

        GameObject point = new GameObject("BingoMapPoint");

        for (int y = mapMatrix.Length - 1; y >= 0; y--)
        {
            for(int x = 0; x < mapMatrix[y].Length; x++)
            {
                Vector3 newPosition = new Vector3(startX + x * _distanceX, startY + y * _distanceY, 0);
                Instantiate(point, newPosition, Quaternion.identity, map.transform);
            }
        }
    }
}
