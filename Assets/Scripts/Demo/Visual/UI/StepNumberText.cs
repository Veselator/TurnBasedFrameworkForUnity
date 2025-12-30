using TMPro;
using UnityEngine;

public class StepNumberText : MonoBehaviour
{
    [SerializeField] private TMP_Text _linkedText;
    private TBS_TurnsManager _turns;

    public void Init()
    {
        _turns = TBS_TurnsManager.Instance;
        _turns.OnTurnChanged += HandleTurnChanged;
        HandleTurnChanged(0);
    }

    private void HandleTurnChanged(int turn)
    {
        _linkedText.text = $"Turn {turn}";
    }

    private void OnDestroy()
    {
        _turns.OnTurnChanged -= HandleTurnChanged;
    }
}
