using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public GameObject back;
    private Desk desk;
    private Button button;

    public float animDuration = 0.25f;
    public float scale = 0.5f;
    
    private float animAmplitude = 5f;
    private float animPeriod = 1f;


    private void Start()
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

    public Tween tween;
    private void CardAnimation(float endScale, float duration)
    {
        tween = gameObject.transform.DOScale(0, duration).SetEase(Ease.InOutElastic, animAmplitude, animPeriod).OnComplete(() =>
        {
            back.SetActive(!back.activeInHierarchy);
            gameObject.transform.DOScale(endScale, duration).SetEase(Ease.InOutElastic, animAmplitude, animPeriod);
        });
    }

    public void InitAnim()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.transform.DOScale(scale, animDuration * 2).SetEase(Ease.InOutElastic, animAmplitude, animPeriod);
    }

    private void OnDestroy()
    {
        DOTween.Kill(this.gameObject);
    }
}
