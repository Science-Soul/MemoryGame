using Assets.Scripts;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Game/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    public ResourceModel.ResourceType primaryResource; // Какой из 7 ресурсов даем
    public int rewardAmount = 10;        // Сколько даем за одну пару
}
