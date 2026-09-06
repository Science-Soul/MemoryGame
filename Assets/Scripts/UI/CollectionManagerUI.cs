using UnityEngine;

public class CollectionManagerUI : MonoBehaviour
{
    public static CollectionManagerUI instance;

    [System.Serializable]
    public struct TabBind
    {
        public GameObject pageObject;
        public CollectionButtonUI tabButton;
    }

    [Header("Связки вкладок и кнопок")]
    [SerializeField] private TabBind[] tabs;

    [Header("Вкладка по умолчанию")]
    [SerializeField] private CollectionButtonUI defaultTab;

    private void Awake()
    {
        instance = this;

        gameObject.SetActive(false);
    }

    private void Start()
    {
        if (defaultTab != null)
        {
            OpenTab(defaultTab);
        }
    }

    // Метод принимает конкретную нажатую КНОПКУ, никаких циклов при наведении!
    public void OpenTab(CollectionButtonUI clickedButton)
    {
        foreach (var tab in tabs)
        {
            if (tab.tabButton != null && tab.pageObject != null)
            {
                // Проверяем, принадлежит ли эта страница нажатой кнопке
                bool isTarget = (tab.tabButton == clickedButton);

                // Включаем/выключаем страницу
                tab.pageObject.SetActive(isTarget);

                // Говорим кнопке обновить свой цвет (она сама знает как)
                tab.tabButton.SetState(isTarget);
            }
        }
    }
}
