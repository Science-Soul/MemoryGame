using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    private Desk desk;
    private GameObject back;
    private Button button;

    private void Awake()
    {
        desk = FindAnyObjectByType<Desk>();
        back = gameObject.transform.Find("back").gameObject;
        back.SetActive(false);

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => desk.OnCardClicked(this.gameObject));
    }
    public void OpenCard()
    {
        back.SetActive(true); 
        button.interactable = false;
    }
    public void CloseCard()
    {
        back.SetActive(false);
        button.interactable = true;
    }
}
