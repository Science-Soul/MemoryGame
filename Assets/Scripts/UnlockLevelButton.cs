using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class UnlockLevelButton : MonoBehaviour
{
    private Button _button;
    [SerializeField] LevelSettings _levelSettings;
    [Inject] private LevelUnlockService _levelUnlockService;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(() => _levelUnlockService.UnlockLevel(_levelSettings));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
