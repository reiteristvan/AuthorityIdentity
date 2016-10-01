using System;
using System.Linq;

namespace AuthorityIdentity.IntegrationTests
{
    public static class RandomData
    {
        private static Random Random = new Random();

        public static string RandomString(int length = 12, bool includeSpecialCharacters = false)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string specialChars = "<>#&!+-*";

            string result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[Random.Next(s.Length)]).ToArray());

            if (includeSpecialCharacters)
            {
                result += specialChars[Random.Next(specialChars.Length)];
                result += specialChars[Random.Next(specialChars.Length)];
            }

            return result;
        }

        public static string Email()
        {
            return RandomString(8) + "@" + RandomString(6) + "." + RandomString(3);
        }
    }
}
