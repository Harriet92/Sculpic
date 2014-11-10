using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Assets.Sources.DatabaseClient.Security
{
    public class SecureString
    {
        private const string salt = "WeAllLoveTaijAndTaio";
        private static SHA256 sha = SHA256.Create();

        public SecureString()
        {
            sha = SHA256.Create();
        }

        public static string GetBase64Hash(string valueToHash)
        {
            string original = String.Concat(salt, valueToHash);
            return Convert.ToBase64String(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(original)));
        }

    }
}
