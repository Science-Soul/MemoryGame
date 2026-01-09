using UnityEngine;
using UnityEngine.UI;

public class CardLogic : MonoBehaviour
{
    public GameObject back;
    private Desk desk;
    private Button button;

    private void Awake()
    {
        desk = FindAnyObjectByType<Desk>();
        back = gameObject.transform.Find("back").gameObject;
        back.SetActive(true);

        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(() => desk.OnCardClicked(this.gameObject));
    }
    public void OpenCard()
    {
        back.SetActive(false); 
        button.interactable = false;
    }
    public void CloseCard()
    {
        back.SetActive(true);
        button.interactable = true;
    }
}
