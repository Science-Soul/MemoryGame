using Assets.Scripts;
using UnityEngine;
using static Assets.Scripts.ResourceModel;
using UnityEngine.AddressableAssets;
using System;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public LevelResourceType levelType; // Какой из 7 ресурсов собираем на уровне
    public int rewardAmount = 10;        // Сколько даем за одну пару

    private bool _isUnlocked = false;
    public bool IsUnlocked()
    {
        return _isUnlocked;
    }

    public void SetUnlocked()
    {
        _isUnlocked = true;
    }

    public AssetReference sceneReference; // Для загрузки сцены через Addressables
    [Serializable]
    public struct LevelCost
    {
        public LevelResourceType type;
        public int amount;
    }

    public LevelCost[] resourcesCost;
    
}
