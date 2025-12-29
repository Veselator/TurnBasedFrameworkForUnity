using UnityEngine;

public class BingoLoadManager : MonoBehaviour
{
    // Отвечает за порядок загрузки модулей в демке

    private void Start()
    {
        TBS_InitManager.Instance.Init();
        PlayerControlls.Instance.Init();
    }
}
