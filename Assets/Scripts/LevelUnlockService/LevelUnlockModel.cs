using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Zenject;

public class LevelUnlockModel
{
    private List<BonusCardDeckSO> _bonusCardDecks = new();
    [Inject] ResourceModel _resources;
    [Inject] private ISaveStorage _storage;
    private SaveData _saveData;

    public void Initialize(BonusCardDeckSO deck)
    {
        _saveData = _storage.Load();
        _bonusCardDecks.Add(deck);
    }

    public bool IsUnlocked(LevelSettings level)
    {
        return _saveData.UnlockedLevels.Contains(level.levelType);
    }

    private bool IsDeckComplete(LevelSettings level)
    {
        //return level.bonusCardDeckSO.bonusCards.Count == _saveData.UnlockedCardNames.Count;
        return level.bonusCardDeckSO.bonusCards.All(c => _saveData.UnlockedCardNames.Contains(c.name));
    }

    public bool TryUnlockLevel(LevelSettings level)
    {
        if (IsUnlocked(level))
        {
            Debug.Log("Уровень уже разблокирован!");
            return false;
        }

        if (IsResourcesEnough(level))
        {
            foreach (var cost in level.resourcesCost)
            {
                _resources.TrySpendResource(cost.type, cost.amount);
            }

            level.SetUnlocked();

            Debug.Log($"<color=green>Уровень {level.levelType} теперь доступен!</color>");

            if (!IsUnlocked(level))
            {
                _saveData.UnlockedLevels.Add(level.levelType);
                _storage.Save(_saveData);
            }
            return true;
        }

        return false;
    }

    public bool IsResourcesEnough(LevelSettings level)
    {
        return level.resourcesCost.All(cost => _resources.GetResourceAvailable(cost.type) >= cost.amount) && IsDeckComplete(level);
    }
}
