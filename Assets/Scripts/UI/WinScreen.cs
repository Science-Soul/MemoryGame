using DG.Tweening;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;

    private CanvasGroup canvasGroup;

    public void ShowWinScreen(string timeText)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        resultText.text = "Вы закончили за " + timeText;
        canvasGroup.DOFade(1, 1);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }
}
