using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // Список имен разблокированных карт
    public List<string> UnlockedCardNames = new List<string>();

    public int Gold = 0;
    public int Food = 0;
    public int Materials = 0;
    public int Seasons = 0;
    public int Science = 0;
    public int Prediction = 0;
    public int Mana = 0;
}
