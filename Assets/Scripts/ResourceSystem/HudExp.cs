using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HudExp : MonoBehaviour
{
    public TextMeshProUGUI masteryLevelText;
    public TextMeshProUGUI masteryRankText;
    //public TextMeshProUGUI expText;
    public Slider expSlider;

    private void Start()
    {
        UpdateUI();
    }

    public void UpdateExpUI(float newExp, int oldExpForLevelUp, int newExpForLevelUp)
    {
        //this.expText.text = ((int)newExp).ToString();
        expSlider.value = (newExp - oldExpForLevelUp) / (newExpForLevelUp - oldExpForLevelUp);
    }

    public void UpdateMasteryText(int newLevel, string newMasteryText)
    {
        this.masteryLevelText.text = "Уровень мастерства " + newLevel.ToString();
        this.masteryRankText.text = newMasteryText;
    }

    public void UpdateUI()
    {
        UpdateExpUI(PlayerPrefs.GetFloat("exp_saved"), PlayerPrefs.GetInt("previousExpForLevelUp_saved", 0), PlayerPrefs.GetInt("currentExpForLevelUp_saved", 0));
        UpdateMasteryText(PlayerPrefs.GetInt("level_saved", 1), PlayerPrefs.GetString("mastery_saved"));
    }

    public void RestartLevel()
    {
        DOTween.KillAll();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}
