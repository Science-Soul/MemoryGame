using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Assets.Scripts
{
	public class BonusDeckButtonInit : MonoBehaviour
	{
        [SerializeField] BonusCardDeckSO deckToUnlock;
        private Button button;

        // Use this for initialization
        void Start()
		{
            ButtonInit();
		}

        private void ButtonInit()
        {
            button = GetComponent<Button>();
            if (deckToUnlock.IsComplete)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
        }
    }
}