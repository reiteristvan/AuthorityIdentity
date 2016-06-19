using System.Security.Cryptography;

namespace Authority.Operations.Security
{
    public sealed class PasswordService
    {
        public byte[] CreateSalt()
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[24];
            csprng.GetBytes(salt);
            return salt;
        }

        public byte[] CreateHash(byte[] password, byte[] salt)
        {
            byte[] hash = PBKDF2(password, salt, 20000, 24);
            return hash;
        }

        public bool ValidatePassword(byte[] hashInDb, byte[] passwordFromUser, byte[] salt)
        {
            byte[] hashFromUser = CreateHash(passwordFromUser, salt);

            uint diff = (uint)hashFromUser.Length ^ (uint)hashInDb.Length;

            for (int i = 0; i < hashFromUser.Length && i < hashInDb.Length; ++i)
            {
                diff |= (uint)(hashFromUser[i] ^ hashInDb[i]);
            }

            return diff == 0;
        }

        private byte[] PBKDF2(byte[] password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
