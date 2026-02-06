using DG.Tweening;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI expText;
    [SerializeField] TextMeshProUGUI bonusText;

    private CanvasGroup canvasGroup;

    public void ShowWinScreen(int expForLevel, int bonus)
    {
        canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(true);
        canvasGroup.alpha = 0;
        expText.text = "Опыт: " + expForLevel;
        bonusText.text = "Бонус за время: " + bonus;
        canvasGroup.DOFade(1, 1);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }
}
