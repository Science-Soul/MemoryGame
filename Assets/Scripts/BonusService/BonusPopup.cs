using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BonusPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button closeButton;

    private Tween _rotationTween;
    private SignalBus _signalBus;

    private GameObject modelToRotate;
    private GameObject _currentModel;

    private DiContainer _container;

    [Inject]
    public void Construct(SignalBus signalBus, CollectionService collectionService, DiContainer container)
    {
        //_signalBus = signalBus;
        //_signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);
        _container = container;
    }
    private void OnDestroy()
    {
        //_signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);
        transform.DOKill();
    }

    private void OnBonusCollected()
    {
        //ShowWithCard()
        //ShowAndRotate(this.GetCancellationTokenOnDestroy()).Forget();
    }

    public async UniTask ShowWithCard(BonusCard bonusCard, CancellationToken token)
    {
        if (_currentModel != null) Destroy(_currentModel);

        // Инстанцируем через Zenject прямо в окно
        _currentModel = _container.InstantiatePrefab(bonusCard, this.gameObject.transform);
        modelToRotate = _currentModel;
        // Сбрасываем трансформацию, чтобы модель встала ровно
        _currentModel.transform.localPosition = new Vector3(0, 0, -10);
        _currentModel.transform.localRotation = Quaternion.identity;


        // 4. Запускаем показ окна (пауза, анимация DOTween и т.д.)
        await ShowAndRotate(token);
    }

    public async UniTask ShowAndRotate(CancellationToken token)
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);

        // DOTween: Плавное появление (игнорирует Time.timeScale)
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true).SetLink(canvasGroup.gameObject);

        // DOTween: Вращение 3D модели
        modelToRotate.transform.rotation = Quaternion.Euler(0, -180, 0);

        _rotationTween = modelToRotate.transform.DORotate(new Vector3(0, 180, 0), 1.5f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear)
            .SetUpdate(true)
            .SetLink(canvasGroup.gameObject);

        await closeButton.OnClickAsync(token);

        _rotationTween.Kill();
        await canvasGroup.DOFade(0, 0.2f).SetUpdate(true).SetLink(canvasGroup.gameObject).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
