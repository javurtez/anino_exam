using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReelObject : MonoBehaviour
{
    private bool isSpinning = false;
    private bool isSlowingDown = false;

    [SerializeField]
    private SymbolObject[] resultData;

    [SerializeField]
    private ReelData[] reelData;

    [SerializeField]
    private VerticalLayoutGroup verticalLayoutGroup;

    [SerializeField, Range(12, 18)]
    private float minReelSpeed = 3;
    [SerializeField, Range(18, 24)]
    private float maxReelSpeed = 3;

    private float reelSpeed = 3;

    [SerializeField]
    private Transform reelParent;

    private IEnumerator delaySpinEnum;

    public SymbolObject[] Result => resultData;

    private void Start()
    {
        for (int i = 0; i < reelData.Length; i++)
        {
            var reel = reelData[i];
            var nextReel = i - 1 >= 0 ? reelData[i - 1] : reelData[^1];
            reel.Apply(nextReel.symbolObject);
        }

        SpinManager.SpinAction += StartSpin;
    }
    private void OnDestroy()
    {
        SpinManager.SpinAction -= StartSpin;
    }
    private void Update()
    {
        if (!isSpinning && !isSlowingDown) return;
        Spinning();
    }

    private void StartSpin(bool isSpin)
    {
        if (isSpin)
        {
            delaySpinEnum = DelaySpin();
            StartCoroutine(delaySpinEnum);
        }
        else if (isSpinning && !isSpin)
        {
            StopCoroutine(delaySpinEnum);
            isSlowingDown = true;
        }
    }
    private IEnumerator DelaySpin()
    {
        reelSpeed = Random.Range(minReelSpeed, maxReelSpeed);
        yield return new WaitForSeconds(Random.Range(.2f, .4f));
        verticalLayoutGroup.enabled = false;
        isSpinning = true;
        isSlowingDown = false;

        yield return new WaitForSeconds(Random.Range(1.3f, 1.8f));
        isSlowingDown = true;
    }
    private void Spinning()
    {
        for (int i = 0; i < reelData.Length; i++)
        {
            var reel = reelData[i];
            var symbol = reel.symbolObject;
            symbol.Rect.position += reelSpeed * Time.deltaTime * Vector3.up;

            if (symbol.Rect.anchoredPosition.y >= 35)
            {
                symbol.UpdatePosition();

                if (isSlowingDown)
                {
                    isSlowingDown = false;
                    isSpinning = false;

                    ClampResultOfSpin();
                    GetResultOfSpin();
                }
            }
        }
    }

    private void ClampResultOfSpin()
    {
        verticalLayoutGroup.enabled = true;
    }
    private void GetResultOfSpin()
    {
        resultData = new SymbolObject[3];
        for (int i = resultData.Length - 1; i >= 0; i--)
        {
            resultData[i] = reelParent.GetChild(4 - i).GetComponent<SymbolObject>();
        }

        SpinManager.Instance.GetResult(this, transform.GetSiblingIndex());
    }
}
