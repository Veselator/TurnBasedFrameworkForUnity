using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class GameEndScreen : MonoBehaviour
{
    private GlobalFlags _globalFlags;

    [SerializeField] private GameObject _handler;
    [SerializeField] private UniversalAnimator _background;
    [SerializeField] private UniversalAnimator _winText;
    [SerializeField] private UniversalAnimator _loseText;

    private void Start()
    {
        _globalFlags = TBS_InitManager.Instance.GlobalFlags;

        _globalFlags.OnGameEnded.AddListener(HandleGameEnded);
    }


    private void OnDestroy()
    {
        if(_globalFlags != null)
        {
            _globalFlags.OnGameEnded.RemoveListener(HandleGameEnded);
        }
    }

    private void HandleGameEnded(List<IPlayer> players)
    {
        
    }
}
