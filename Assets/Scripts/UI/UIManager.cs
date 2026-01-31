using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI masteryText;
    public TextMeshProUGUI expText;
    public Slider expSlider;
    public LevelObjectives levelObjectives;
    public Timer timer;
    public WinScreen winScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateExpUI(string newExpText, int oldExpForLevelUp, int newExpForLevelUp, float currentExp)
    {
        this.expText.text = newExpText;
        expSlider.value = (currentExp - oldExpForLevelUp) / (newExpForLevelUp - oldExpForLevelUp);
    }

    public void UpdateMasteryText(string newMasteryText)
    {
        this.masteryText.text = newMasteryText;
    }
}
