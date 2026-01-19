using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSettingsHandler : MonoBehaviour
{
    public string Name { get; private set; }
    public bool IsAi { get; private set; }
    public PiecePrefabInfo ChosenPiecePrefab { get; private set; }
    public int AiDifficultyLevel { get; private set; }

    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private Toggle _isAiToggle;
    [SerializeField] private Slider _aiDifficultySlider;
    private GameObject _sliderAiDifficultyGameObject;
    [SerializeField] private Image _selectedPiecePlaceholder;
    [SerializeField] private Button _leftPieceButton;
    [SerializeField] private Button _rightPieceButton;

    private PieceSelectorManager _piecesSelectorManager;
    private bool _isBinded = false;

    public void Init(string name, bool isAi, PiecePrefabInfo chosenPiecePrefab, int aiDifficultyLevel, PieceSelectorManager psm)
    {
        _piecesSelectorManager = psm;
        Name = name;
        IsAi = isAi;
        ChosenPiecePrefab = chosenPiecePrefab;
        AiDifficultyLevel = aiDifficultyLevel;

        _sliderAiDifficultyGameObject = _aiDifficultySlider.gameObject;

        _nameField.text = Name;
        _isAiToggle.isOn = IsAi;
        _aiDifficultySlider.value = AiDifficultyLevel;
        UpdatePieceVisual();
        UpdateAiDifficultyVisibility();

        Bind();
    }

    private void Bind()
    {
        _isBinded = true;
        _nameField.onValueChanged.AddListener(OnNameChanged);
        _isAiToggle.onValueChanged.AddListener(OnIsAiToggled);
        _aiDifficultySlider.onValueChanged.AddListener(OnAiDifficultyChanged);
        _leftPieceButton.onClick.AddListener(OnLeftPieceButtonClicked);
        _rightPieceButton.onClick.AddListener(OnRightPieceButtonClicked);
    }

    private void Unbind()
    {
        if (!_isBinded) return;
        _isBinded = false;
        _nameField.onValueChanged.RemoveListener(OnNameChanged);
        _isAiToggle.onValueChanged.RemoveListener(OnIsAiToggled);
        _aiDifficultySlider.onValueChanged.RemoveListener(OnAiDifficultyChanged);
        _leftPieceButton.onClick.RemoveListener(OnLeftPieceButtonClicked);
        _rightPieceButton.onClick.RemoveListener(OnRightPieceButtonClicked);
    }

    private void OnDestroy()
    {
        Unbind();
    }

    private void OnNameChanged(string name)
    {
        Name = name;
    }

    private void OnIsAiToggled(bool isAI)
    {
        IsAi = isAI;
        UpdateAiDifficultyVisibility();
    }

    private void OnRightPieceButtonClicked()
    {
        ChosenPiecePrefab = _piecesSelectorManager.SelectNextPiece(this, 1);
        UpdatePieceVisual();
    }

    private void OnLeftPieceButtonClicked()
    {
        ChosenPiecePrefab = _piecesSelectorManager.SelectNextPiece(this, -1);
        UpdatePieceVisual();
    }

    private void OnAiDifficultyChanged(float difficultyLevel)
    {
        AiDifficultyLevel = (int)difficultyLevel;
    }

    private void UpdateAiDifficultyVisibility()
    {
        _sliderAiDifficultyGameObject.SetActive(IsAi);
    }

    private void UpdatePieceVisual()
    {
        if (ChosenPiecePrefab.Icon != null)
            _selectedPiecePlaceholder.sprite = ChosenPiecePrefab.Icon;
    }
}