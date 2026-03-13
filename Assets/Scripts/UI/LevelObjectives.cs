using TMPro;
using UnityEngine;

public class LevelObjectives : MonoBehaviour
{
    private TextMeshProUGUI levelObjText;

    public void Init(string s)
    {
        levelObjText = GetComponent<TextMeshProUGUI>();
        levelObjText.text = s;
    }
}
