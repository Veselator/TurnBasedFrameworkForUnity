using UnityEngine;
using UnityEngine.SceneManagement;

public class ParallaxEffect : MonoBehaviour
{
    [Header("Parallax Settings")]
    private Transform cameraTransform;
    [SerializeField] private float parallaxFactor = 0.5f;
    [SerializeField] private bool parallaxX = true;
    [SerializeField] private bool parallaxY = false;

    private Vector3 lastCameraPosition;

    private void Start()
    {
        cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // Применяем параллакс
        Vector3 parallaxMovement = Vector3.zero;

        if (parallaxX)
        {
            parallaxMovement.x = deltaMovement.x * parallaxFactor;
        }

        if (parallaxY)
        {
            parallaxMovement.y = deltaMovement.y * parallaxFactor;
        }

        transform.position += parallaxMovement;
        lastCameraPosition = cameraTransform.position;
    }
}