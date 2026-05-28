using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Zenject;

public class LevelUnlockModel
{
    [Inject] ResourceModel _resources;
    [Inject] private ISaveStorage _storage;
    private LevelSettings _level;
    private bool isLevelUnlocked;

    public LevelUnlockModel(LevelSettings level)
    {
        _level = level;
        Debug.Log("Level init");
    }

    public bool IsUnlocked()
    {
        //return _storage.Load().UnlockedLevels.Contains(level.levelType);
        return _level.IsUnlocked();
    }

    public bool TryUnlockLevel()
    {
        if (IsUnlocked())
        {
            Debug.Log("Уровень уже разблокирован!");
            return false;
        }

        if (IsResourcesEnough())
        {
            foreach (var cost in _level.resourcesCost)
            {
                _resources.TrySpendResource(cost.type, cost.amount);
            }

            _level.SetUnlocked();
            /*var data = _storage.Load();
            data.UnlockedLevels.Add(level.levelType);
            _storage.Save(data);*/

            Debug.Log($"<color=green>Уровень {_level.levelType} теперь доступен!</color>");
            return true;
        }

        return false;
    }

    public bool IsResourcesEnough()
    {
        Debug.Log("level " + _level);
        Debug.Log("resources " + _resources);
        return _level.resourcesCost.All(cost => _resources.GetResourceAvailable(cost.type) >= cost.amount);
    }
}
