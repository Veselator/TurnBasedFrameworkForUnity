using System.Text;
using TMPro;
using UnityEngine;

public class PointsText : MonoBehaviour
{
    [SerializeField] private TMP_Text _linkedText;
    private GlobalFlags _globalFlags;
    private TBS_PlayersManager _playersManager;

    public void Init()
    {
        _globalFlags = TBS_InitManager.Instance.GlobalFlags;
        _playersManager = TBS_PlayersManager.Instance;

        _globalFlags.OnRoundEnded.AddListener(HandleRoundChanged);
        HandleRoundChanged(null);
    }

    private void HandleRoundChanged(RuleWinResult _)
    {
        _linkedText.text = $"Points:\n{GetPointsAndPlayers()}";
    }

    private string GetPointsAndPlayers()
    {
        StringBuilder sb = new StringBuilder();
        foreach (var Player in _playersManager.Players)
        {
            sb.AppendLine($"{Player.Name} {Player.OverallScore}");
        }

        return sb.ToString();
    }

    private void OnDestroy()
    {
        _globalFlags.OnRoundEnded.RemoveListener(HandleRoundChanged);
    }
}
