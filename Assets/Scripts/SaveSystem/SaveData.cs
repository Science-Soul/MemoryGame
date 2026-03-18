using System.Collections.Generic;
using Assets.Scripts;

[System.Serializable]
public class SaveData
{
    // Список имен разблокированных карт
    public List<string> UnlockedCardNames = new();

    // Список разблокированных уровней
    public List<ResourceModel.LevelResourceType> UnlockedLevels = new() { ResourceModel.LevelResourceType.GOLD};

    public int Gold = 0;
    public int Food = 0;
    public int Materials = 0;
    public int Seasons = 0;
    public int Science = 0;
    public int Prediction = 0;
    public int Mana = 0;
}
