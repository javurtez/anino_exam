using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class SpinManager : MonoBehaviour
{
    [SerializeField]
    private PayoutScriptableObject[] payoutScriptableObjects;
    [SerializeField]
    private LineScriptableObject[] lineScriptableObjects;

    [Space(10)]

    [SerializeField]
    private ReelObject[] resultObjects; // Result being passed

    [SerializeField]
    private LineRenderer lineRenderer; // Linerenderer for the line indicator of the reel

    public Dictionary<string, PayoutScriptableObject> payoutScriptableDicts;

    private bool isSpinning = false;
    private bool canSpin = true;
    private int spinCount = 0;

    public bool IsSpinning => isSpinning;

    public delegate void ActionBoolDelegate(bool action);
    public static ActionBoolDelegate SpinAction;

    public static SpinManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        CheckIfLineHasDuplicate();
        payoutScriptableDicts = new Dictionary<string, PayoutScriptableObject>();
        // Add in the dictionary
        foreach (var obj in payoutScriptableObjects)
        {
            payoutScriptableDicts.Add(obj.symbol.name, obj);
        }
    }

    public void GetResult(ReelObject resultData, int index)
    {
        canSpin = false;
        isSpinning = false;
        spinCount--;

        resultObjects[index] = resultData;

        // When spin count is 0, gets the line result
        if (spinCount == 0)
        {
            StartCoroutine(GetLineResult());
            //int multiplier = await GetLineResult();
        }
    }
    private IEnumerator GetLineResult()
    {
        int result = 0;
        yield return new WaitForSeconds(.1f);
        // Loops all line matrix
        for (int i = 0; i < lineScriptableObjects.Length; i++)
        {
            var line = lineScriptableObjects[i];
            var lineResult = new string[5];
            var symbolResult = new SymbolObject[5];
            Debug.Log(line.name);
            for (int j = 0; j < line.rows.Length; j++)
            {
                var row = line.rows[j];
                // Gets the result based on the line row
                symbolResult[j] = resultObjects[j].Result[row];
                lineResult[j] = symbolResult[j].Result;
            }

            int add = GetRewardMultiplier(lineResult, symbolResult);
            result += add;

            // No delay when add is 0
            if (add != 0)
            {
                yield return new WaitForSeconds(.8f);
            }
        }
        canSpin = true;
        SpinAction.Invoke(isSpinning);
        lineRenderer.positionCount = 0;

        Debug.Log("Result: " + result);

        PlayerManager.Instance.Reward(result);
    }
    private int GetRewardMultiplier(string[] result, SymbolObject[] symbolResult)
    {
        // Check for duplicates and add how many
        var duplicates = result
            .GroupBy(x => x)
            .Where(g => g.Count() > 1)
            .Select(g => new { Word = g.Key, Count = g.Count() });

        int multiplier = 0;

        bool hasDuplicateGreaterThan2 = false;
        foreach (var duplicate in duplicates)
        {
            Debug.Log($"Word '{duplicate.Word}' has {duplicate.Count} occurrences.");
            multiplier += payoutScriptableDicts[duplicate.Word].Payout(duplicate.Count);

            if (duplicate.Count > 2)
            {
                hasDuplicateGreaterThan2 = true;
            }
        }

        if (hasDuplicateGreaterThan2)
        {
            lineRenderer.positionCount = 5;
            for (int i = 0; i < symbolResult.Length; i++)
            {
                var symbol = symbolResult[i];
                var pos = symbol.transform.position;
                pos.z = 0;
                // Set line position
                lineRenderer.SetPosition(i, pos);

                // Juicing: Scale up and revert back for eye candy
                LeanTween.scale(symbol.Rect, Vector2.one * 1.1f, .25f).
                    setEase(LeanTweenType.linear).
                    setLoopPingPong(1);
            }

            AudioManager.Instance.PlayLine();
        }
        return multiplier;
    }

    public void Spin()
    {
        if (!canSpin) return;
        if (!isSpinning)
        {
            if (!PlayerManager.Instance.HasBet) return;
            if (!PlayerManager.Instance.HasEnoughCoins()) return;
        }

        AudioManager.Instance.PlayLever();

        isSpinning = !isSpinning;
        Debug.Log(isSpinning);
        SpinAction.Invoke(isSpinning);
        if (isSpinning)
        {
            spinCount = 5;
            resultObjects = new ReelObject[5];
            PlayerManager.Instance.DeductCoins();
        }
    }

    private void CheckIfLineHasDuplicate()
    {
        for (int i = 0; i < lineScriptableObjects.Length; i++)
        {
            if (i + 1 >= lineScriptableObjects.Length) break;
            for (int j = 0; j < lineScriptableObjects.Length - 1; j++)
            {
                if (j + 1 == i) continue;
                bool areEqual = AreArraysEqual(lineScriptableObjects[i].rows, lineScriptableObjects[j + 1].rows);
                if (areEqual)
                {
                    Debug.LogError($"Has an equal Line Matrix {lineScriptableObjects[i].name} & {lineScriptableObjects[j + 1].name}");
                }
            }
        }
    }
    private bool AreArraysEqual<T>(T[] array1, T[] array2)
    {
        if (array1 == null && array2 == null)
        {
            // Both arrays are null, consider them equal
            return true;
        }

        if (array1 == null || array2 == null || array1.Length != array2.Length)
        {
            // Arrays have different lengths or one of them is null, consider them not equal
            return false;
        }

        for (int i = 0; i < array1.Length; i++)
        {
            if (!EqualityComparer<T>.Default.Equals(array1[i], array2[i]))
            {
                // Found a different value in the arrays
                return false;
            }
        }

        // All values in the arrays are the same
        return true;
    }
}
