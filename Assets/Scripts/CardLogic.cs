using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public GameObject back;
    private Desk desk;
    private Button button;

    [SerializeField] float animDuration = 0.25f;
    [SerializeField] float scale = 0.5f;

    private void Awake()
    {
        desk = FindAnyObjectByType<Desk>();
        back = gameObject.transform.Find("back").gameObject;
        back.SetActive(true);

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => desk.OnCardClicked(this.gameObject));
    }
    public void TurnOverCard()
    {
        button.interactable = !button.interactable;
        CardAnimation(scale, animDuration);
    }

    private void CardAnimation(float endScale, float duration)
    {
        gameObject.transform.DOScale(0, duration).SetEase(Ease.OutElastic).OnComplete(() =>
        {
            back.SetActive(!back.activeInHierarchy);
            gameObject.transform.DOScale(endScale, duration).SetEase(Ease.InElastic);
        });
    }

    private void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }
}
