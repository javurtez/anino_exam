using TMPro;
using UnityEngine;

public class PlayerPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI coinText;

    [SerializeField]
    private TextMeshProUGUI betText;

    [SerializeField]
    private TextMeshProUGUI winText;

    [SerializeField]
    private TextMeshProUGUI spinText;

    private void Awake()
    {
        PlayerManager.CoinUpdate += CoinUpdate;
        PlayerManager.BetUpdate += BetUpdate;
        PlayerManager.RewardUpdate += RewardUpdate;
        SpinManager.SpinAction += SpinUpdate;
    }
    private void OnDestroy()
    {
        PlayerManager.CoinUpdate -= CoinUpdate;
        PlayerManager.BetUpdate -= BetUpdate;
        PlayerManager.RewardUpdate -= RewardUpdate;
        SpinManager.SpinAction -= SpinUpdate;
    }

    private void CoinUpdate(int coins)
    {
        coinText.SetText(coins.ToString());

        // Animate when coin value changes
        LeanTween.scale(coinText.gameObject, Vector3.one * 1.1f, .1f).
            setEase(LeanTweenType.linear).
            setLoopPingPong(1);
    }
    private void BetUpdate(int coins)
    {
        betText.SetText(coins.ToString());
    }
    private void RewardUpdate(int coins)
    {
        winText.SetText(coins.ToString());
    }
    private void SpinUpdate(bool isSpinning)
    {
        spinText.text = !isSpinning ? "SPIN" : "STOP";
    }
}
