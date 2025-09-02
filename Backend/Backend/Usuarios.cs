using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text.Json.Serialization;

public class Usuarios
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }

    [JsonConstructor]
    public Usuarios(string email, string password, string name)
    {
        Email = email;
        Name = name;
        Password = password;
        
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