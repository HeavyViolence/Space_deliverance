using System.Collections.Generic;
using UnityEngine;

public static class AuxMath
{
    public const float E = 2.718_281_828f;
    public const float Phi = 1.618_033_988f;
    public const float SquareRootOf2 = 1.414_213_562f;

    public static float RandomSign => Random.Range(-1f, 1f) < 0f ? -1f : 1f;

    public static bool RandomBool => RandomSign > 0f;

    public static int EnsurePositiveValue(int value) => Mathf.Clamp(value, 0, int.MaxValue);

    public static float EnsurePositiveValue(float value) => Mathf.Clamp(value, 0f, float.MaxValue);

    public static float RangeRemap(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        float clampedValue = Mathf.Clamp(value, oldMin, oldMax);
        float lerpfactor = Mathf.InverseLerp(oldMin, oldMax, clampedValue);

        return Mathf.Lerp(newMin, newMax, lerpfactor);
    }

    public static int GetRandomFromRangeWithExclusions(int minInclusive, int maxExclusive, IEnumerable<int> exclusions)
    {
        var excludedIntegers = new HashSet<int>(exclusions);

        int availableIntegersAmount = maxExclusive - minInclusive - excludedIntegers.Count;
        var availableIntegers = new List<int>(availableIntegersAmount);

        for (int i = minInclusive; i < maxExclusive; i++)
        {
            if (excludedIntegers.Contains(i))
            {
                continue;
            }

            availableIntegers.Add(i);
        }

        int randomIndex = Random.Range(0, availableIntegers.Count);

        return availableIntegers[randomIndex];
    }
}
