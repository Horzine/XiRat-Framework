using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;

public class HashCalculator : MonoBehaviour
{
    private void Start()
    {
        string str = CalculateSHA256("./.gitignore");
        Debug.Log(str);

        bool rst = VerifySHA256Hash("./.gitignore", str);
        Debug.Log(rst);
    }

    public static string CalculateSHA256(string filePath)
    {
        using var sha256 = SHA256.Create();
        using var fileStream = File.OpenRead(filePath);
        byte[] hash = sha256.ComputeHash(fileStream);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    public static bool VerifySHA256Hash(string filePath, string expectedHash)
    {
        string actualHash = CalculateSHA256(filePath);
        return string.Equals(actualHash, expectedHash, StringComparison.OrdinalIgnoreCase);
    }
}