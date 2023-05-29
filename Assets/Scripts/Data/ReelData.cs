using UnityEngine;

[System.Serializable]
public class ReelData
{
    public SymbolObject symbolObject;
    public Sprite symbolSprite;

    public void Apply(SymbolObject nextReel)
    {
        symbolObject.ChangeSymbol(symbolSprite);
        symbolObject.ChangeNextReel(nextReel);
    }
}
