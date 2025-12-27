using UnityEngine;

public class TBS_RoundEndHandler : MonoBehaviour
{
    // Держатель конца раунда
    // Отвечает за перезагрузку данных
    // и подготовку к следующему раунду

    // Правила AfterRound

    public static TBS_RoundEndHandler Instance {  get; private set; }
    private GlobalFlags _globalFlags;

    private void Awake()
    {
        Instance = this;
    }

    public void Init(GlobalFlags globalFlags)
    {
        _globalFlags = globalFlags;
        _globalFlags.OnRoundEnded.AddListener(HandleRoundEnd);
    }

    private void OnDestroy()
    {
        if (_globalFlags != null)
        {
            _globalFlags.OnRoundEnded.RemoveListener(HandleRoundEnd);
        }
    }

    private void HandleRoundEnd(int roundResult)
    {
        // TODO: перезагрузка данных, проверка правил
        _globalFlags.TriggerNextRoundAllowed();
    }
}
