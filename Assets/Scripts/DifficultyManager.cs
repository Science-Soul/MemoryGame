using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class DifficultyManager : MonoBehaviour
{
    [Inject] private ISaveStorage _storage;
    public List<ChooseDifficultyButton> buttons = new();

    public int LevelDifficultiesCount;

    private void Awake()
    {
        var data = _storage.Load();
        LevelDifficultiesCount = buttons.Count;

        for (int i = 1; i <= data.UnlockedLevelDifficulties; i++)
        {
            if (buttons.Count >= i)
            {
                buttons[i - 1].Unlock();
            }
            else
            {
                return;
            }
        }
    }
}
