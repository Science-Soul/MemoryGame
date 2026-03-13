using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChooseDifficultyButton : MonoBehaviour
{
    [Inject] SignalBus _signalBus;
    [SerializeField] DifficultyLevels difficultyLevel;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            _signalBus.Fire(new StartGameSignal { selectedDifficulty = this.difficultyLevel });
            transform.root.gameObject.SetActive(false);
        });
    }
}
