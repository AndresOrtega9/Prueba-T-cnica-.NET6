using System.Security.Cryptography;
using System.Text;

namespace CATALOGO_DE_PRODUCTOS.Utilidades
{
    public class Utilidades
    {
        public static string EncriptarPassword(string password)
        {
            string hash = string.Empty;
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashvalue = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                foreach (byte b in hashvalue)
                {
                    hash += $"{b:X2}";
                }
            }
            return hash;
        }
    }
}
