using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Assets.Scripts
{
    public class DeckToggleButton : MonoBehaviour
    {
        public AssetReference scene;
        [SerializeField] Deck deckData;
        [SerializeField] Button button;

        private AsyncOperationHandle<SceneInstance> _currentSceneHandle;

        private void Start()
        {
            button.onClick.AddListener( () => LoadScene(this.GetCancellationTokenOnDestroy()));
        }

        private void OnDestroy()
        {
            button.onClick?.RemoveAllListeners();
        }

        public void LoadScene(CancellationToken token)
        {
            ChangeSceneAsync(token).Forget();
        }

        public async UniTaskVoid ChangeSceneAsync(CancellationToken token)
        {
            button.interactable = false;

            if (_currentSceneHandle.IsValid())
            {
                await Addressables.UnloadSceneAsync(_currentSceneHandle);
                Debug.Log("Старая сцена и её ресурсы выгружены");
            }

            _currentSceneHandle = Addressables.LoadSceneAsync(scene);

            // Ждем завершения загрузки
            await _currentSceneHandle;

            if (_currentSceneHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("<color=green>Новая сцена успешно загружена и готова!</color>");
            }
            else
            {
                Debug.LogError("Ой! Что-то пошло не так при загрузке.");
            }

            if (button != null)
            {
                button.interactable = true;
            }
        }
    }
}