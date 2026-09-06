using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CollectionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image buttonImage;

    [Header("Настройки цвета")]
    [SerializeField] private Color normalColor = Color.gray;
    [SerializeField] private Color hoverColor = Color.yellow;
    [SerializeField] private Color activeColor = Color.white;

    public bool IsActive { get; private set; }


    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void SetState(bool active)
    {
        IsActive = active;
        buttonImage.color = active ? activeColor : normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!IsActive)
        {
            buttonImage.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsActive)
        {
            buttonImage.color = normalColor;
        }
    }

    public void OnButtonClick()
    {
        if (CollectionManagerUI.instance != null)
        {
            CollectionManagerUI.instance.OpenTab(this);
        }
    }
}
