using System;
using System.Linq;

namespace Authority.IntegrationTests
{
    public static class RandomData
    {
        public static string RandomString(int length = 12, bool includeSpecialCharacters = false)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string specialChars = "<>#&!+-*";

            Random random = new Random();

            string result = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            if (includeSpecialCharacters)
            {
                result += specialChars[random.Next(specialChars.Length)];
                result += specialChars[random.Next(specialChars.Length)];
            }

            return result;
        }

        public static string Email()
        {
            return RandomString(8) + "@" + RandomString(6) + "." + RandomString(3);
        }
    }
}
