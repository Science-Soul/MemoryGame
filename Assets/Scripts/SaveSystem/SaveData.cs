using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    // Список имен разблокированных карт
    public List<string> UnlockedCardNames = new List<string>();

    public int Gold;
    public int Wood;
}
