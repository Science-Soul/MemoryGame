using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Zenject;

public class LevelUnlockModel
{
    [Inject] private ResourceModel _resources;
    [Inject] private ISaveStorage _storage;
    private bool isLevelUnlocked;

    public bool IsUnlocked(LevelSettings level)
    {
        //return _storage.Load().UnlockedLevels.Contains(level.levelType);
        return level.IsUnlocked();
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
            /*var data = _storage.Load();
            data.UnlockedLevels.Add(level.levelType);
            _storage.Save(data);*/

            Debug.Log($"<color=green>Уровень {level.levelType} теперь доступен!</color>");
            return true;
        }

        return false;
    }

    public bool IsResourcesEnough(LevelSettings level)
    {
        return level.resourcesCost.All(cost => _resources.GetResourceAvailable(cost.type) >= cost.amount);
    }
}
