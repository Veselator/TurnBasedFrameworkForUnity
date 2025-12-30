using UnityEngine;

public class BingoLoadManager : MonoBehaviour
{
    // Отвечает за порядок загрузки модулей в демке

    [SerializeField] private BingoVisualMapGenerator _mapGenerator;
    [SerializeField] private BingoVisualUserPointer _userPointer;

    [SerializeField] private StepNumberText _stepNumberText;
    [SerializeField] private TurnText _turnText;
    [SerializeField] private PointsText _pointsText;
    [SerializeField] private RoundText _roundText;

    private void Start()
    {
        TBS_InitManager.Instance.Init();
        PlayerControlls.Instance.Init();
        VisualPieceFactory.Instance.Init(TBS_InitManager.Instance.InitConfig);
        _mapGenerator.Init();
        _userPointer.Init();

        _stepNumberText.Init();
        _turnText.Init();
        _pointsText.Init();
        _roundText.Init();

        TBS_InitManager.Instance.StartGame();
    }
}
