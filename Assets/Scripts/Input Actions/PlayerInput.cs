using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Общая система ввода для игрока
    // Использует Unity Input System

    // Взято из CourseProject2025

    private PlayerInputAction _playerInputActions;
    private PlayerInputAction.PlayerActions _player;
    public PlayerInputAction playerInputAction => _playerInputActions;

    public static PlayerInput Instance { get; private set; }

    void Awake()
    {
        _playerInputActions = new PlayerInputAction();
        _playerInputActions.Enable();
        _player = _playerInputActions.Player;

        Instance = this;
    }

    public Vector2 GetMovementVector()
    {
        return _player.Move.ReadValue<Vector2>().normalized;
    }

    private void OnDestroy()
    {
        _playerInputActions.Disable();
    }

    public bool IsHitButtonPressed() // Spacebar
    {
        return _playerInputActions.Player.Jump.triggered;
    }
}
