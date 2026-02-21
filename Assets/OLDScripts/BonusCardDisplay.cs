using UnityEngine;
using UnityEngine.U2D;

public class BonusCardDisplay : MonoBehaviour
{
    private Sprite _spriteFace;
    private Sprite _spriteBack;

    [SerializeField] Renderer _rendererFace;
    [SerializeField] Renderer _rendererBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }


    public void Init(Sprite spriteFace, Sprite spriteBack)
    {
        _spriteFace = spriteFace;
        ApplyToQuad(_spriteFace, _rendererFace);
        //_rendererFace.material.mainTexture = spriteFace.texture;

        _spriteBack = spriteBack;
        ApplySpriteWithPropertyBlock(_spriteBack, _rendererBack);
        //ApplyToQuad(_spriteBack, _rendererBack);
        //_rendererBack.material.mainTexture = spriteBack.texture;

    }

    void ApplyToQuad(Sprite s, Renderer rend)
    {
        rend.material.mainTexture = s.texture;

        // Рассчитываем UV-координаты конкретного спрайта в атласе
        // Используем textureRect, чтобы найти положение спрайта на листе
        Rect r = s.textureRect;
        Vector2 size = new Vector2(r.width / s.texture.width, r.height / s.texture.height);
        Vector2 offset = new Vector2(r.x / s.texture.width, r.y / s.texture.height);

        rend.material.mainTextureScale = size;
        rend.material.mainTextureOffset = offset;
    }

    public void ApplySpriteWithPropertyBlock(Sprite s, Renderer renderer)
    {
        // 1. Создаем "блок свойств"
        MaterialPropertyBlock block = new MaterialPropertyBlock();

        // 2. Рассчитываем координаты (как раньше)
        Rect r = s.textureRect;
        float texW = s.texture.width;
        float texH = s.texture.height;

        Vector4 tilingOffset = new Vector4(
            r.width / texW,
            r.height / texH,
            r.x / texW,
            r.y / texH
        );

        // 3. Записываем данные в блок (имена свойств стандартные для Unity)
        // _MainTex_ST — это стандартное имя для Tiling/Offset в шейдерах
        block.SetVector("_MainTex_ST", tilingOffset);

        // 4. Применяем блок к рендереру
        renderer.SetPropertyBlock(block);

        // ВАЖНО: Убедись, что в Renderer.sharedMaterial назначен твой ФАЙЛ материала рубашки
    }
}
