using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine;
using System.Text;
using System;
using System.Security.Cryptography;

public static class AuxMath
{
    public const float E = 2.718_281_828f;
    public const float Phi = 1.618_033_988f;
    public const float SquareRootOf2 = 1.414_213_562f;

    public static float RandomSign => UnityEngine.Random.Range(-1f, 1f) < 0f ? -1f : 1f;

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

        int randomIndex = UnityEngine.Random.Range(0, availableIntegers.Count);

        return availableIntegers[randomIndex];
    }

    public static string SerializeObjectToString(object obj)
    {
        using MemoryStream memoryStream = new();
        using StreamReader reader = new(memoryStream, Encoding.UTF8);
        DataContractSerializer serializer = new(obj.GetType());

        serializer.WriteObject(memoryStream, obj);
        memoryStream.Position = 0;

        return reader.ReadToEnd();
    }

    public static T DeserializeStringToObject<T>(string objectAsXml, IEnumerable<Type> knownTypes)
    {
        using MemoryStream memoryStream = new();
        byte[] xmlAsBytes = Encoding.UTF8.GetBytes(objectAsXml);
        DataContractSerializer deserializer = new(typeof(T), knownTypes);

        memoryStream.Write(xmlAsBytes, 0, xmlAsBytes.Length);
        memoryStream.Position = 0;
        
        if (deserializer.ReadObject(memoryStream) is T value)
        {
            return value;
        }
        else
        {
            throw new Exception("Passed data is invalid or corrupted and cannot be properly restored!");
        }
    }

    public static string EncodeOrDecode(string value, string key)
    {
        StringBuilder builder = new(value.Length);

        for (int i = 0; i < value.Length; i++)
        {
            builder.Append((char)(value[i] ^ key[i % key.Length]));
        }

        return builder.ToString();
    }

    public static string Encode(string input, string publicKey, string privateKey)
    {
        try
        {
            byte[] inputAsBytes = Encoding.UTF8.GetBytes(input);
            byte[] publicKeyAsBytes = Encoding.UTF8.GetBytes(publicKey);
            byte[] privateKeyAsBytes = Encoding.UTF8.GetBytes(privateKey);

            using DESCryptoServiceProvider cryptoServiceProvider = new();
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream,
                                                  cryptoServiceProvider.CreateEncryptor(publicKeyAsBytes, privateKeyAsBytes),
                                                  CryptoStreamMode.Write);

            cryptoStream.Write(inputAsBytes, 0, inputAsBytes.Length);
            cryptoStream.Close();

            return Convert.ToBase64String(memoryStream.ToArray());
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

    public static string Decode(string encodedInput, string publicKey, string privateKey)
    {
        try
        {
            byte[] encodedInputAsBytes = Convert.FromBase64String(encodedInput);
            byte[] publicKeyAsBytes = Encoding.UTF8.GetBytes(publicKey);
            byte[] privateKeyAsBytes = Encoding.UTF8.GetBytes(privateKey);

            using DESCryptoServiceProvider cryptoServiceProvider = new();
            using MemoryStream memoryStream = new();
            using CryptoStream cryptoStream = new(memoryStream,
                                                  cryptoServiceProvider.CreateDecryptor(publicKeyAsBytes, privateKeyAsBytes),
                                                  CryptoStreamMode.Write);

            cryptoStream.Write(encodedInputAsBytes, 0, encodedInputAsBytes.Length);
            cryptoStream.Close();

            return Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }
}