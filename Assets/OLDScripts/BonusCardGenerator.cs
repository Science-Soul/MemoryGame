using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D;
using System.Linq;

public class BonusCardGenerator : MonoBehaviour
{
    //[SerializeField] SpriteAtlas _atlas;
    [SerializeField] Transform _gridTransform;
    [SerializeField] GameObject _bonusCardPrefab;

    [SerializeField] Sprite[] _spritesFace;
    [SerializeField] Sprite _spriteBack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateDeck();
    }

    private void GenerateDeck()
    {

        foreach (var spriteFace in _spritesFace)
        {
            var card = Instantiate(_bonusCardPrefab, _gridTransform);
            card.name = spriteFace.name;
            BonusCardDisplay display = card.GetComponent<BonusCardDisplay>();
            display.Init(spriteFace, _spriteBack);
        }
    }
}
