using UnityEngine;
using UnityEngine.UI;

public class ReloadSceneOnClick : MonoBehaviour
{
    [SerializeField] private Button _linkedButton;

    private void Start()
    {
        _linkedButton.onClick.AddListener(OnButtonClick);
    }

    private void OnDestroy()
    {
        _linkedButton.onClick.RemoveListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        GameSceneManager.ReloadScene();
    }
}
