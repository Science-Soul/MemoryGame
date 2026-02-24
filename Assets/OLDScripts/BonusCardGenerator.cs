using UnityEngine;

public class BonusCardGenerator : MonoBehaviour
{
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
            Destroy(card.GetComponent<BonusCardDisplay>());
        }
    }
}
