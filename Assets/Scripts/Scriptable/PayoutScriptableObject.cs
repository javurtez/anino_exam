using UnityEngine;

[CreateAssetMenu(fileName = "My PayoutScriptableObject", menuName = "Scriptable/Payout")]
public class PayoutScriptableObject : ScriptableObject
{
    public Sprite symbol;

    public int[] payoutMultiplier;

    public int Payout(int amount)
    {
        return payoutMultiplier[amount - 1];
    }
}