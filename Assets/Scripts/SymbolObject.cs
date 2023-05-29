using UnityEngine;
using UnityEngine.UI;

public class SymbolObject : MonoBehaviour
{
    [SerializeField]
    private Image symbolImage;

    private SymbolObject nextReel;
    private RectTransform rectTransform;
    private string result;

    public RectTransform Rect => rectTransform;
    // Key result are string
    public string Result => result;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void ChangeSymbol(Sprite dataSprite)
    {
        symbolImage.sprite = dataSprite;

        // Update result value when symbol changes
        result = symbolImage.sprite.name;
    }
    public void ChangeNextReel(SymbolObject next)
    {
        nextReel = next;
    }

    public void UpdatePosition()
    {
        Rect.anchoredPosition = nextReel.Rect.anchoredPosition - new Vector2(0, 75);

        // Moves last when position is being update/moved to the last
        transform.SetAsLastSibling();
    }
}
