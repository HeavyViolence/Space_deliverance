using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using System.Text;

public static class AuxMath
{
    public const float E = 2.718_281_828f;
    public const float Phi = 1.618_033_988f;
    public const float SquareRootOf2 = 1.414_213_562f;

    public static float RandomSign => Random.Range(-1f, 1f) < 0f ? -1f : 1f;

    public static bool RandomBool => RandomSign > 0f;

    public static float Remap(float value, float oldMin, float oldMax, float newMin, float newMax)
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

    public static string SerializeObject(object obj)
    {
        using MemoryStream memoryStream = new();
        using StreamReader reader = new(memoryStream);
        DataContractSerializer serializer = new(obj.GetType());

        serializer.WriteObject(memoryStream, obj);
        memoryStream.Position = 0;

        return reader.ReadToEnd();
    }

    public static T DeserializeObject<T>(string xml)
    {
        using MemoryStream memoryStream = new();
        byte[] data = Encoding.UTF8.GetBytes(xml);
        DataContractSerializer deserializer = new(typeof(T));

        memoryStream.Write(data, 0, data.Length);
        memoryStream.Position = 0;

        return (T)deserializer.ReadObject(memoryStream);
    }

    public static string EncodeOrDecode(string value, string key)
    {
        StringBuilder builder = new(value.Length);

        for (int i = 0; i < value.Length; i++)
        {
            builder.Append(value[i] ^ key[i % key.Length]);
        }

        return builder.ToString();
    }
}