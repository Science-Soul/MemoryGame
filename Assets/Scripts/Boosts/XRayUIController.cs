using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class XRayUIController : MonoBehaviour
{
    [SerializeField] Material xrayMaterial; // Общий материал для всех рубашек
    [SerializeField] float defaultRadius = 150f;

    private bool _isActive = true;
    private Camera _uiCamera;

    [Inject]
    public void Construct(Camera uiCamera) => _uiCamera = uiCamera;

    public void ToggleXRay(bool state)
    {
        _isActive = state;
        // Если выключили — уводим центр в бесконечность
        if (!state) xrayMaterial.SetVector("_Center", new Vector4(-10000, -10000, 0, 0));
    }

    private void Update()
    {
        if (!_isActive) return;

        // Передаем позицию мыши (в экранных координатах) в шейдер
        Vector2 mousePos = Mouse.current.position.ReadValue();

        // В Shader Graph Screen Position идет от 0 до Resolution. 
        // Если Canvas в Overlay — используем Input.mousePosition напрямую.
        Shader.SetGlobalVector("_GlobalMousePos", new Vector4(mousePos.x, mousePos.y, 0, 0));
        Shader.SetGlobalFloat("_GlobalRadius", defaultRadius);
    }
}
