using UnityEngine;
using UnityEditor; // Работает только в Editor
using System.IO;

public class CardBaker : EditorWindow
{
    [MenuItem("Tools/Bake Cards to Prefabs")]
    public static void Bake()
    {
        // 1. Выдели все сгенерированные карты на сцене
        GameObject[] selectedCards = Selection.gameObjects;

        if (selectedCards.Length == 0)
        {
            Debug.LogWarning("Сначала выдели объекты карт на сцене!");
            return;
        }

        string folderPath = "Assets/BakedCards";
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        foreach (GameObject card in selectedCards)
        {
            // 2. Создаем уникальный материал для этой карты, 
            // иначе у всех префабов будет одна и та же последняя текстура.
            Renderer renFace = card.transform.Find("FaceQuad").GetComponent<Renderer>();
            if (renFace != null)
            {
                // Клонируем материал и сохраняем его как ассет
                Material newMat = new Material(renFace.sharedMaterial);
                string matPath = $"{folderPath}/Materials/{card.name}_Mat.mat";
                if (!Directory.Exists($"{folderPath}/Materials")) Directory.CreateDirectory($"{folderPath}/Materials");
                AssetDatabase.CreateAsset(newMat, matPath);

                // Назначаем сохраненный материал объекту
                renFace.sharedMaterial = newMat;
            }

            // 3. Сохраняем объект как префаб
            string prefabPath = $"{folderPath}/{card.name}.prefab";
            PrefabUtility.SaveAsPrefabAssetAndConnect(card, prefabPath, InteractionMode.UserAction);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Карты успешно запечены в " + folderPath);
    }
}
