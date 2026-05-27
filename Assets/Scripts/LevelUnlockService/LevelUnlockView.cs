using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;
using static LevelSettings;

public class LevelUnlockView : MonoBehaviour
{
    [SerializeField] Button _unlockLevelButton;
    private TMP_Text _unlockLevelText;
    [SerializeField] Button _runLevelButton;
    [SerializeField] LevelSettings _levelSettings;
    [Inject] private LevelUnlockModel _levelUnlockModel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //_button = GetComponent<Button>();
        /*_unlockLevelText = _unlockLevelButton.GetComponentInChildren<TMP_Text>();
        _unlockLevelText.color = Color.black;*/
        _unlockLevelText.text = string.Join("\n", _levelSettings.resourcesCost.Select(x => $"{x.type}: {x.amount}"));

        /*if (_levelUnlockModel.IsUnlocked(_levelSettings))
        {
            _unlockLevelButton.gameObject.SetActive(false);
        }
        else if (_levelUnlockModel.IsResourcesEnough(_levelSettings))
        {
            _unlockLevelButton.onClick.AddListener(() =>
            {
                if (_levelUnlockModel.TryUnlockLevel(_levelSettings))
                {
                    _unlockLevelButton.gameObject.SetActive(false);
                }
            });
        }
        else
        {
            text.color = Color.red;
            _unlockLevelButton.interactable = false;
        }*/

    }

    private void OnDestroy()
    {
        _unlockLevelButton.onClick.RemoveAllListeners();
    }

    public LevelSettings GetLevelSettings()
    {
        return _levelSettings;
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
}
