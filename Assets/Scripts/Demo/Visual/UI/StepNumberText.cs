using TMPro;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class StepNumberText : MonoBehaviour
{
    [SerializeField] private TMP_Text _linkedText;
    private TBS_TurnsManager _turns;

    public void Start()
    {
        _turns = TBS_TurnsManager.Instance;
        _turns.OnTurnChanged += HandleTurnChanged;
        HandleTurnChanged(0);
    }

    private void HandleTurnChanged(int turn)
    {
        _linkedText.text = $"Step {turn + 1}";
    }

    private void OnDestroy()
    {
        _turns.OnTurnChanged -= HandleTurnChanged;
    }
}
