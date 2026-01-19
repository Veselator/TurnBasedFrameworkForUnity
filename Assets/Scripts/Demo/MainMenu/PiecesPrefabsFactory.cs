using UnityEngine;

public class PiecesPrefabsFactory : MonoBehaviour
{
    [SerializeField] private PiecePrefabInfo[] _piecePrefabs;

    public int PiecesCount => _piecePrefabs.Length;

    public PiecePrefabInfo GetPrefabById(int id)
    {
        if (id < 0 || id >= _piecePrefabs.Length)
        {
            Debug.LogError($"PiecesPrefabsFactory: Invalid id {id}");
            return default;
        }
        return _piecePrefabs[id];
    }

    public int GetIdByPrefab(PiecePrefabInfo prefabInfo)
    {
        for (int i = 0; i < _piecePrefabs.Length; i++)
        {
            if (_piecePrefabs[i].Prefab == prefabInfo.Prefab)
                return i;
        }
        return -1;
    }
}

[System.Serializable]
public struct PiecePrefabInfo
{
    public GameObject Prefab;
    public Sprite Icon;
}