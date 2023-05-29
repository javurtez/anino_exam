using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private int coins;

    [SerializeField]
    private int currentBet;

    public bool HasBet => currentBet > 0;

    public delegate void ValueDelegate(int value);
    public static ValueDelegate CoinUpdate;
    public static ValueDelegate BetUpdate;
    public static ValueDelegate RewardUpdate;

    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;

        Application.targetFrameRate = 60;
    }
    private void Start()
    {
        CoinUpdate?.Invoke(coins);
        BetUpdate?.Invoke(currentBet);
    }

    public bool HasEnoughCoins(int newBet)
    {
        return coins >= newBet;
    }
    public bool HasEnoughCoins()
    {
        return coins >= currentBet;
    }

    public void AddCoins(int coin)
    {
        coins += coin;
        CoinUpdate?.Invoke(coins);

        AudioManager.Instance.PlayCoin();
    }
    public void DeductCoins()
    {
        coins -= currentBet;

        CoinUpdate?.Invoke(coins);
    }

    public void AddBet(int coin)
    {
        int newBet = Mathf.Clamp(currentBet + coin, 0, coins);
        if (!HasEnoughCoins(newBet))
        {
            newBet = coins;
        }
        currentBet = newBet;

        BetUpdate?.Invoke(currentBet);

        AudioManager.Instance.PlayBet();
    }
    public void DeductBet(int coin)
    {
        int newBet = Mathf.Clamp(currentBet - coin, 0, coins);
        if (!HasEnoughCoins(newBet)) return;
        currentBet = newBet;

        BetUpdate?.Invoke(currentBet);

        AudioManager.Instance.PlayBet();
    }

    public void Reward(int multiplier)
    {
        int totalReward = currentBet * multiplier;
        if (totalReward != 0)
        {
            AddCoins(totalReward);
        }

        RewardUpdate?.Invoke(totalReward);
    }
}
