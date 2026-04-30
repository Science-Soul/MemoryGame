using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Zenject;

namespace Assets.Scripts
{
	public class BonusDeckButtonInit : MonoBehaviour
	{
        [SerializeField] BonusCardDeckSO deckToUnlock;
        [SerializeField] ResourceModel.LevelResourceType type;
        private Button button;

        [Inject] ISaveStorage _storage;
        private SaveData saveData;
        // Use this for initialization
        void Start()
		{
            saveData = _storage.Load();
            ButtonInit();
		}

        private void ButtonInit()
        {
            button = GetComponent<Button>();
            if (deckToUnlock.IsComplete && saveData.UnlockedLevels.Contains(type))
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