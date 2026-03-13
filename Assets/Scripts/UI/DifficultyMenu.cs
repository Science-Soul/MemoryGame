using UnityEngine;
using Zenject;
using UnityEngine.UI;

public class DifficultyMenu : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] DifficultyLevels difficultyLevel;
    [SerializeField] Button[] buttons;

    private void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                _signalBus.Fire(new StartGameSignal { selectedDifficulty =  this.difficultyLevel});
                gameObject.SetActive(false); // Скрываем меню
            });
        }
    }
}
