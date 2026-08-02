using Assets.Scripts;
using R3;
using TMPro;
using UnityEngine;
using Zenject;

public class HudView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;
    [SerializeField] TextMeshProUGUI foodText;
    [SerializeField] TextMeshProUGUI materialsText;
    [SerializeField] TextMeshProUGUI seasonsText;
    [SerializeField] TextMeshProUGUI scienceText;
    [SerializeField] TextMeshProUGUI predictionText;
    [SerializeField] TextMeshProUGUI manaText;

    private ResourceModel _model;
    private readonly CompositeDisposable _disposables = new();

    [Inject]
    public void Construct(ResourceModel model)
    {
        _model = model;
    }

    private void Start()
    {
        // Подписка через R3: автоматически обновляет текст при изменении значения
        _model.Gold
            .Subscribe(value => goldText.text = $"{value}")
            .AddTo(_disposables);

        _model.Food
            .Subscribe(value => foodText.text = $"{value}")
            .AddTo(_disposables);

        _model.Materials
            .Subscribe(value => materialsText.text = $"{value}")
            .AddTo(_disposables);

        _model.Prediction          
            .Subscribe(value => predictionText.text = $"{value}")
            .AddTo(_disposables);

        _model.Science
            .Subscribe(value => scienceText.text = $"{value}")
            .AddTo(_disposables);

        _model.Seasons
            .Subscribe(value => seasonsText.text = $"{value}")
            .AddTo(_disposables);

        _model.Mana
            .Subscribe(value => manaText.text = $"{value}")
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
