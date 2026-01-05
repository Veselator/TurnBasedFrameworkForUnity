using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BingoVisualMap : MonoBehaviour
{
    // Отвечает за визуализацию карты
    private Transform[][] _points;
    private BingoMap _map;

    private List<GameObject> _pieces;

    // Анимация появления фишек
    [SerializeField] private float _pieceAnimationSpeed = 1.0f;
    [SerializeField] private float _offsetYToSpawnPieces = -1f;
    [SerializeField] private float _offsetYToDisappearPieces = -10f;
    [SerializeField] private float _endRoundAnimationDuration = 1.3f;

    public float ToppestY => _points[_points.Length - 1][0].position.y;

    public static BingoVisualMap Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void Init(Transform[][] p, BingoMap map)
    {
        _points = p;
        _map = map;
        _pieces = new List<GameObject>();

        // Передаём через конфиг в фабрику данные про визуал фишек
        _map.OnElementAdded += HandleElementAdded;
        _map.OnMapReset += HandleMapReset;
    }

    private void OnDestroy()
    {
        if (_points != null)
        {
            _map.OnElementAdded -= HandleElementAdded;
            _map.OnMapReset -= HandleMapReset;
        }
    }

    public float GetXByColumnId(int columnId)
    {
        if(columnId < 0 || columnId >= _points[0].Length) return 0;
        return _points[0][columnId].position.x;
    }

    public float GetYByRowId(int rowId)
    {
        if (rowId < 0 || rowId >= _points.Length) return 0;
        return _points[rowId][0].position.y;
    }

    private void HandleElementAdded(int x, int y, int playerId)
    {
        GameObject piecePrefab = VisualPieceFactory.GetPiece(playerId);
        if(piecePrefab == null)
        {
            Debug.LogError("Чё-то не то с фабрикой либо индексом");
            return;
        }

        GameObject piece = Instantiate(piecePrefab);
        Transform currentPoint = _points[y][x];
        piece.transform.position = new Vector2(currentPoint.position.x, _points[_points.Length - 1][0].position.y + _offsetYToSpawnPieces);

        piece.AddComponent<UniversalAnimator>().Animate(currentPoint.position, _pieceAnimationSpeed);
        _pieces.Add(piece);
    }

    private void HandleMapReset()
    {
        int[] order = new int[_pieces.Count];
        for (int i = 0; i < order.Length; i++)
        {
            order[i] = i;
        }

        var rng = new System.Random();
        for (int i = order.Length - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (order[i], order[j]) = (order[j], order[i]);
        }

        StartCoroutine(ClearMap(order, _endRoundAnimationDuration));
    }

    private IEnumerator ClearMap(int[] order, float totalTimeToClean)
    {
        if (order.Length == 0)
        {
            _pieces.Clear();
            yield break;
        }

        if (order.Length == 1)
        {
            _pieces[order[0]].GetComponent<UniversalAnimator>()
                .AnimateWithOffset(new Vector2(0, _offsetYToDisappearPieces), _pieceAnimationSpeed, true);
            _pieces.Clear();
            yield break;
        }

        float elapsed = 0f;
        int currentIndex = 0;

        while (currentIndex < order.Length)
        {
            float progress = (float)currentIndex / (order.Length - 1);

            float easedProgress = (float)Math.Sqrt(progress);

            float targetTime = easedProgress * totalTimeToClean;

            while (elapsed < targetTime)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }

            _pieces[order[currentIndex]].GetComponent<UniversalAnimator>()
                .AnimateWithOffset(new Vector2(0, _offsetYToDisappearPieces), _pieceAnimationSpeed, true);

            currentIndex++;
        }

        _pieces.Clear();
    }
}
