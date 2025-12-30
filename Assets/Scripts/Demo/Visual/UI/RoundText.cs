using TMPro;
using UnityEngine;

public class RoundText : MonoBehaviour
{
    [SerializeField] private TMP_Text _linkedText;
    private GlobalFlags _globalFlags;

    private int _maxRounds;

    public void Init()
    {
        _globalFlags = TBS_InitManager.Instance.GlobalFlags;
        _maxRounds = TBS_TurnsManager.Instance.MaxRounds;

        _globalFlags.OnRoundStarted.AddListener(HandleRoundChanged);
        HandleRoundChanged(0);
    }

    private void HandleRoundChanged(int round)
    {
        _linkedText.text = $"Round {round + 1}/{_maxRounds}";
    }

    private void OnDestroy()
    {
        _globalFlags.OnRoundStarted.RemoveListener(HandleRoundChanged);
    }
}
