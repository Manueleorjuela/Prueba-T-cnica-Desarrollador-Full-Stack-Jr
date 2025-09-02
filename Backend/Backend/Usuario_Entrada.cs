using System.Security.Cryptography;

namespace Backend
{
    public class Usuario_Entrada
    {
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public (byte[] PasswordHash, byte[] PasswordSalt) Convertir_Contraseña_Texto(string plainPassword)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            using (var pbkdf2 = new Rfc2898DeriveBytes(plainPassword, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return (hashBytes, saltBytes);
            }
        }
        public (byte[] PasswordHash, byte[] PasswordSalt) Convertir_Contraseña(string plainPassword)
        {
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            using (var pbkdf2 = new Rfc2898DeriveBytes(plainPassword, saltBytes, 100000, HashAlgorithmName.SHA256))
            {
                byte[] hashBytes = pbkdf2.GetBytes(32);
                return (hashBytes, saltBytes);
            }
        }

    }
}
