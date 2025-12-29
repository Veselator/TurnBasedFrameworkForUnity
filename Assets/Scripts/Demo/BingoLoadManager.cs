using UnityEngine;

public class BingoLoadManager : MonoBehaviour
{
    // Отвечает за порядок загрузки модулей в демке

    [SerializeField] private BingoVisualMapGenerator _mapGenerator;
    [SerializeField] private BingoVisualUserPointer _userPointer;

    private void Start()
    {
        TBS_InitManager.Instance.Init();
        PlayerControlls.Instance.Init();
        VisualPieceFactory.Instance.Init(TBS_InitManager.Instance.InitConfig);
        _mapGenerator.Init();
        _userPointer.Init();

        TBS_InitManager.Instance.StartGame();
    }
}
