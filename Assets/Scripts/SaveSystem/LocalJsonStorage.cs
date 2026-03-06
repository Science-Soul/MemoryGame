using System.IO;
using UnityEngine;

public class LocalJsonStorage : ISaveStorage
{
    private string _path => Path.Combine(Application.persistentDataPath, "save.json");

    private SaveData _cachedData;

    public void Save(SaveData data)
    {
        // Обновляем кэш перед записью (на случай, если передали новый объект)
        _cachedData = data;

        // Запись на диск (это можно делать реже, если нужно, но для JSON — ок)
        string json = JsonUtility.ToJson(_cachedData, true);
        File.WriteAllText(_path, json);
        Debug.Log($"<color=magenta>[SaveSystem]</color> Сохранено в: {_path}");
    }

    public SaveData Load()
    {
        // 1. Если данные уже в памяти — отдаем их мгновенно
        if (_cachedData != null) return _cachedData;

        // 2. Если файла нет — создаем пустой объект и кэшируем его
        if (!File.Exists(_path))
        {
            _cachedData = new SaveData();
            return _cachedData;
        }
        // 3. Читаем с диска только ОДИН раз за запуск игры
        string json = File.ReadAllText(_path);
        _cachedData = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("<color=cyan>[SaveStorage]</color> Файл прочитан с диска и закэширован.");
        return _cachedData;
    }
}
