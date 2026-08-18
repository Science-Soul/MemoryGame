using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;

public class ExitButton : MonoBehaviour
{
    public AssetReference sceneReference;

    public void BackToMainMenu()
    {
        Addressables.LoadSceneAsync(sceneReference, LoadSceneMode.Single);
    }
}
