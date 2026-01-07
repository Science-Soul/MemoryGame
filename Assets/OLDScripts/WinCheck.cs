using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinCheck : MonoBehaviour
{
    public TMP_Text lvlText;
    private int lvl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lvl = PlayerPrefs.GetInt("lvl", 0);
        UpdateSkillText();
        Invoke(nameof(CheckCards), 3f);
    }

    void CheckCards()
    {
        GameObject[] cards = GameObject.FindGameObjectsWithTag("Card");

        if (cards.Length == 0)
        {
            lvl++;
            PlayerPrefs.SetInt("lvl", lvl);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            Invoke(nameof(CheckCards), 1f);
        }
    }

    void UpdateSkillText()
    {
        lvlText.text = $"LVL: {lvl}";
    }
}
