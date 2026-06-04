using UnityEngine;
using UnityEngine.UI;

public class ButtonThreshold : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
