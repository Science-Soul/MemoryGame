using Assets.Scripts;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

public class HudView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI woodText;

    private ResourceModel _model;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public void Construct(ResourceModel model)
    {
        _model = model;
    }

    private void Start()
    {
        // ѕодписка через R3: автоматически обновл€ет текст при изменении значени€
        _model.Gold
            .Subscribe(value => goldText.text = $"Gold: {value}")
            .AddTo(_disposables);

        _model.Wood
            .Subscribe(value => woodText.text = $"Wood: {value}")
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
