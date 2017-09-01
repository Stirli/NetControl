using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NetControlCommon.Utils
{
    public static class HashGenerator
    {
        public static string GetHash<T>(this string input) where T:HashAlgorithm, new()
        {
            HashAlgorithm hashAlg = new T();
            // Convert the input string to a byte array and compute the hash.
            byte[] data = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyHash<T>(this string input, string hash) where T:HashAlgorithm, new()
        {
            // Hash the input.
            string hashOfInput = GetHash<T>(input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
