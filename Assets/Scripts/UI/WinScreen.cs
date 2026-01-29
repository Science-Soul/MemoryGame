using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void ShowWinScreen(string timeText)
    {
        gameObject.SetActive(true);
        resultText.text = "Вы закончили за " + timeText;
        Time.timeScale = 0;
    }

    private void OnEnable()
    {
        
    }
}
