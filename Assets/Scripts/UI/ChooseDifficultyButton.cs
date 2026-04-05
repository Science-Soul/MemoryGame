using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ChooseDifficultyButton : MonoBehaviour
{
    [Inject] SignalBus _signalBus;
    public DifficultyLevels difficultyLevel;

    public bool isUnlocked = false;

    private void Start()
    {
        Button button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            _signalBus.Fire(new StartGameSignal { selectedDifficulty = this.difficultyLevel });
            transform.root.gameObject.SetActive(false);
        });

        if (isUnlocked)
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void Unlock()
    {
        isUnlocked = true;
    }
}
