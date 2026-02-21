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
    [SerializeField] private Transform modelRoot;
    [SerializeField] private Button closeButton;

    private Tween _rotationTween;
    private SignalBus _signalBus;

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<BonusCollectedSignal>(OnBonusCollected);
    }
    private void OnDestroy() => _signalBus.Unsubscribe<BonusCollectedSignal>(OnBonusCollected);

    private void OnBonusCollected()
    {
        ShowAndRotate(this.GetCancellationTokenOnDestroy()).Forget();
    }

    public async UniTask ShowAndRotate(CancellationToken token)
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);

        // DOTween: Плавное появление (игнорирует Time.timeScale)
        canvasGroup.alpha = 0;
        canvasGroup.DOFade(1, 0.5f).SetUpdate(true);

        // DOTween: Вращение 3D модели
        _rotationTween = modelRoot.DORotate(new Vector3(0, 180, 0), 1.5f, RotateMode.WorldAxisAdd)
            .SetEase(Ease.Linear)
            .SetUpdate(true);

        await closeButton.OnClickAsync(token);

        _rotationTween.Kill();
        await canvasGroup.DOFade(0, 0.2f).SetUpdate(true).AsyncWaitForCompletion();
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
