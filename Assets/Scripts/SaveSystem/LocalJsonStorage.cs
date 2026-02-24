using System.IO;
using UnityEngine;

public class LocalJsonStorage : ISaveStorage
{
    private string _path => Path.Combine(Application.persistentDataPath, "save.json");

    public void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(_path, json);
        Debug.Log($"<color=magenta>[SaveSystem]</color> Сохранено в: {_path}");
    }

    public SaveData Load()
    {
        if (!File.Exists(_path)) return new SaveData();
        string json = File.ReadAllText(_path);
        return JsonUtility.FromJson<SaveData>(json);
    }
}
