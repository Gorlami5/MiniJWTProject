using System.Text;

namespace MiniJWTProject.Helpers
{
    public static class HashHelper
    {
        public static void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt = hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPassword(string password, byte[] userpasswordsalt, byte[] userpasswordhash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(userpasswordsalt))
            {
                var computedhash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedhash.Length; i++)
                {
                    if (computedhash[i] != userpasswordhash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
    }
}
