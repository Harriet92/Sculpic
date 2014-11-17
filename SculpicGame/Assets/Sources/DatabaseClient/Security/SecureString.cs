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
            var hash = UTF8Encoding.UTF8.GetString(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(original)));
            string result = "";
            foreach (byte b in hash)
            {
                result = result + b;
            }
            return result;
        }
    }
}
