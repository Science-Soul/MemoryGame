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
        // ѕодписка через R3: автоматически обновл€ет текст при изменении значени€
        _model.Gold
            .Subscribe(value => goldText.text = $"Gold: {value}")
            .AddTo(_disposables);

        _model.Food
            .Subscribe(value => foodText.text = $"Food: {value}")
            .AddTo(_disposables);

        _model.Materials
            .Subscribe(value => materialsText.text = $"Materials: {value}")
            .AddTo(_disposables);

        _model.Prediction          
            .Subscribe(value => predictionText.text = $"Prediction: {value}")
            .AddTo(_disposables);

        _model.Science
            .Subscribe(value => scienceText.text = $"Science: {value}")
            .AddTo(_disposables);

        _model.Seasons
            .Subscribe(value => seasonsText.text = $"Seasons: {value}")
            .AddTo(_disposables);

        _model.Mana
            .Subscribe(value => manaText.text = $"Mana: {value}")
            .AddTo(_disposables);
    }

    private void OnDestroy()
    {
        _disposables.Dispose();
    }
}
