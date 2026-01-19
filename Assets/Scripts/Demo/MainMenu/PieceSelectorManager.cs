using System.Collections.Generic;
using UnityEngine;

public class PieceSelectorManager : MonoBehaviour
{
    private PiecesPrefabsFactory _piecesPrefabsFactory;
    private Dictionary<PlayerSettingsHandler, int> _selectedPieces = new Dictionary<PlayerSettingsHandler, int>();

    public void Init(PiecesPrefabsFactory factory)
    {
        _piecesPrefabsFactory = factory;
        _selectedPieces.Clear();
    }

    public void RegisterPlayer(PlayerSettingsHandler handler, int initialPieceId)
    {
        _selectedPieces[handler] = initialPieceId;
    }

    public bool IsPieceOccupied(int pieceId, PlayerSettingsHandler excludeHandler)
    {
        foreach (var kvp in _selectedPieces)
        {
            if (kvp.Key != excludeHandler && kvp.Value == pieceId)
                return true;
        }
        return false;
    }

    public PiecePrefabInfo SelectNextPiece(PlayerSettingsHandler handler, int direction)
    {
        if (!_selectedPieces.ContainsKey(handler))
        {
            Debug.LogError("PieceSelectorManager: Handler not registered");
            return default;
        }

        int currentId = _selectedPieces[handler];
        int startId = currentId;
        int totalPieces = _piecesPrefabsFactory.PiecesCount;

        do
        {
            currentId += direction;

            // Циклический переход
            if (currentId >= totalPieces)
                currentId = 0;
            else if (currentId < 0)
                currentId = totalPieces - 1;

            // Если вернулись к начальной - выходим (все заняты)
            if (currentId == startId)
                break;

        } while (IsPieceOccupied(currentId, handler));

        _selectedPieces[handler] = currentId;
        return _piecesPrefabsFactory.GetPrefabById(currentId);
    }

    public int GetCurrentPieceId(PlayerSettingsHandler handler)
    {
        return _selectedPieces.TryGetValue(handler, out int id) ? id : -1;
    }
}