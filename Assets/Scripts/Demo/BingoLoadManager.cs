using UnityEngine;

public class BingoLoadManager : MonoBehaviour
{
    // Отвечает за порядок загрузки модулей в демке

    [SerializeField] private BingoVisualMapGenerator _mapGenerator;

    private void Start()
    {
        TBS_InitManager.Instance.Init();
        PlayerControlls.Instance.Init();
        _mapGenerator.Init();
    }
}
