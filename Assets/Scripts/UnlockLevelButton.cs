using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using System.Linq;

public class UnlockLevelButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] LevelSettings _levelSettings;
    [Inject] private LevelUnlockService _levelUnlockService;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        var text = _button.GetComponentInChildren<TMP_Text>();
        text.color = Color.black;
        text.text = string.Join("\n", _levelSettings.resourcesCost.Select(x => $"{x.type}: {x.amount}"));

        if (_levelUnlockService.IsUnlocked(_levelSettings))
        {
            _button.gameObject.SetActive(false);
        }
        else if (_levelUnlockService.IsResourcesEnough(_levelSettings))
        {
            _button.onClick.AddListener(() =>
            {
                if (_levelUnlockService.TryUnlockLevel(_levelSettings))
                {
                    _button.gameObject.SetActive(false);
                }
            });
        }
        else
        {
            text.color = Color.red;
            _button.interactable = false;
        }

    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }
}
