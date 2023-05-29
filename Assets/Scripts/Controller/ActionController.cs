using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    private bool canSpin = true;

    public void OnIncreaseBet()
    {
        PlayerManager.Instance.AddBet(100);
    }
    public void OnDecreaseBet()
    {
        PlayerManager.Instance.DeductBet(100);
    }

    public void OnSpin()
    {
        if (!canSpin) return;

        canSpin = false;
        // Add delay
        LeanTween.delayedCall(.7f, () =>
        {
            canSpin = true;
        });

        SpinManager.Instance.Spin();
    }
}
