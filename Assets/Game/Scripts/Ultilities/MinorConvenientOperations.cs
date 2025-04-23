using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinorConvenientOperations
{
    public static string GenerateRandomString(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        char[] stringChars = new char[length];
        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[UnityEngine.Random.Range(0, chars.Length)];
        }
        return new string(stringChars);
    }
}
