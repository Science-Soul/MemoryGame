using System.Linq;
using Assets.Scripts;
using UnityEngine;
using Zenject;

public class LevelUnlockModel
{
    [Inject] ResourceModel _resources;
    [Inject] private ISaveStorage _storage;
    private SaveData _saveData;
    private LevelSettings _level;


    public LevelUnlockModel(LevelSettings level)
    {
        _level = level;
        Debug.Log("Level init");
    }

    public void Initialize()
    {
        _saveData = _storage.Load();
    }

    public bool IsUnlocked()
    {
        //return _storage.Load().UnlockedLevels.Contains(level.levelType);
        //return _level.IsUnlocked();
        return _saveData.UnlockedLevels.Contains(_level.levelType);
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

            if (!IsUnlocked())
            {
                _saveData.UnlockedLevels.Add(_level.levelType);
                _storage.Save(_saveData);
            }
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
