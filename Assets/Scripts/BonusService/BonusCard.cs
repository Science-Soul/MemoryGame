using UnityEngine;
using UnityEngine.AddressableAssets;

public class BonusCard : MonoBehaviour
{
    private bool isUnlocked = false;
    public bool IsUnlocked { get { return isUnlocked; } }

    public void UnlockCard()
    {
        isUnlocked = true;
    }
}
