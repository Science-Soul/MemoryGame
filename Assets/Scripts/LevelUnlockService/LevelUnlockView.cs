using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;
using static LevelSettings;
using System;

public class LevelUnlockView : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] Button _unlockLevelButton;
    [SerializeField] Button _runLevelButton;
    private TMP_Text _unlockLevelText;

    //[Inject] private LevelUnlockModel _levelUnlockModel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _unlockLevelText = _unlockLevelButton.GetComponentInChildren<TMP_Text>();
        //_unlockLevelText.text = string.Join("\n", _levelSettings.resourcesCost.Select(x => $"{x.type}: {x.amount}"));
        _unlockLevelButton.onClick.AddListener(() => OnClick?.Invoke());
    }

    public event Action OnClick;

    private void OnDestroy()
    {
        _unlockLevelButton.onClick.RemoveAllListeners();
    }

    public void SetState(bool isUnlocked)
    {
        _runLevelButton.interactable = isUnlocked;
        _unlockLevelButton.gameObject.SetActive(!isUnlocked);
    }

    public void SetInteractable(bool isResourcesEnough)
    {
        _unlockLevelButton.interactable = isResourcesEnough;
        _unlockLevelText = _unlockLevelButton.GetComponentInChildren<TMP_Text>();
        if (isResourcesEnough)
        {
            _unlockLevelText.color = Color.black;
        }
        else
        {
            _unlockLevelText.color = Color.red;
        }
    }

    public void SetText(string text)
    {
        _unlockLevelText.text = text;
    }

    public LevelSettings GetLevelSettings()
    {
        return _levelSettings;
    }
}
