using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts
{
    public class DeckToggleButton : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [SerializeField] private Deck deckData;
        [SerializeField] private Button button;

        private void Start()
        {
            // При нажатии просто кидаем сигнал с данными этой колоды
            button.OnClickAsObservable().Subscribe(_ =>
            {
                _signalBus.Fire(new ChangeDeckSignal { newDeck = deckData });
            }).AddTo(this); // автоотписка
        }
    }
}