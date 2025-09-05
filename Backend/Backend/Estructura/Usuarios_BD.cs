using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Estructura
{
    public class Usuarios_BD
    {
        private readonly Conexion_BD _conexion;
        public Usuarios_BD(Conexion_BD conexion)
        {
            _conexion = conexion;
        }
        public void Registrar_Usuario(Usuarios Usuario)
        {
            var resultado = Usuario.Convertir_Contraseña(Usuario.Password);
            byte[] hash = resultado.PasswordHash;
            byte[] salt = resultado.PasswordSalt;
            string query = @"INSERT INTO Users (Email, PasswordHash, PasswordSalt, Name) 
            VALUES (@Email, @PasswordHash, @PasswordSalt, @Name)";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = Usuario.Email;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, -1).Value = hash;
                cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, -1).Value = salt;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = Usuario.Name;
                cmd.ExecuteNonQuery();
            }
        }

        public bool ObtenerUsuarioPorEmail(string email)
        {
            string query = "SELECT 1 FROM Users WHERE Email = @Email";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
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

        public bool Verificar_Contraseña(string email, string password)
        {
            string query = "SELECT PasswordHash, PasswordSalt FROM Users WHERE Email = @Email";

            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        byte[] storedHash = (byte[])reader["PasswordHash"];
                        byte[] storedSalt = (byte[])reader["PasswordSalt"];

                        using (var pbkdf2 = new Rfc2898DeriveBytes(password, storedSalt, 100000, HashAlgorithmName.SHA256))
                        {
                            byte[] computedHash = pbkdf2.GetBytes(32);
                            return storedHash.SequenceEqual(computedHash);
                        }
                    }
                }
            }
            return false;
        }

        public string Tipo_Usuario(string email)
        {
            string query = "SELECT Role FROM Users WHERE Email = @Email";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    return null;
                }
            }
        }
        public List<Operaciones_Usuarios> ListarUsuarios()
        {
            var lista = new List<Operaciones_Usuarios>();
            string query = "SELECT Id, Email, Name, Role FROM Users";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    lista.Add(new Operaciones_Usuarios
                    {
                        Id = reader.GetGuid(0).ToString(),
                        Email = reader.GetString(1),
                        Name = reader.GetString(2),
                        Role = reader.GetString(3)
                    });
                }
            }
            return lista;
        }
        public bool VerificarID(string ID)
        { 
            if (!Guid.TryParse(ID, out Guid guidID))
                return false; 

            string query = "SELECT 1 FROM Users WHERE Id = @Id";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
        
                cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = guidID;

                using (var reader = cmd.ExecuteReader())
                {
                    return reader.Read();
                }
            }
        }

        public bool ID_Correspiente(string ID, string email)
        {
            string query = "SELECT Id FROM Users WHERE Email = @Email";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                var result = cmd.ExecuteScalar();
                string idEnBD = result.ToString();
                return idEnBD.Equals(ID, StringComparison.OrdinalIgnoreCase);
            }
        }

        public bool VerificarRol(string rol)
        {
            return rol == "user" || rol == "admin";
        }

        public void Eliminar_Usuario(string ID)
        {
            string query = "DELETE FROM Users WHERE Id = @Id";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", ID);
                cmd.ExecuteNonQuery();
            }
        }

        public void Añadir_Usuario_Admin(Usuario_Entrada Usuario)
        {
            var resultado = Usuario.Convertir_Contraseña(Usuario.Password);
            byte[] hash = resultado.PasswordHash;
            byte[] salt = resultado.PasswordSalt;
            string query = @"INSERT INTO Users (Email, PasswordHash, PasswordSalt, Name, Role) 
            VALUES (@Email, @PasswordHash, @PasswordSalt, @Name, @Role)";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = Usuario.Email;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, -1).Value = hash;
                cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, -1).Value = salt;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = Usuario.Name;
                cmd.Parameters.Add("@Role", SqlDbType.NVarChar, 20).Value = Usuario.Role;
                cmd.ExecuteNonQuery();
            }
        }

        public void Ver_Usuario(string ID, Mostrar_Usuario usuario)
        {
            string query = "SELECT * FROM Users WHERE Id = @Id";
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Id", ID);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        usuario.Email = reader.GetString(reader.GetOrdinal("Email"));
                        usuario.Name = reader.GetString(reader.GetOrdinal("Name"));
                        usuario.Role = reader.GetString(reader.GetOrdinal("Role"));
                    }
                }
            }
        }
        public void Editar_Usuario(string ID, Usuario_Entrada usuario, string rol)
        {
            var resultado = usuario.Convertir_Contraseña(usuario.Password);
            byte[] hash = resultado.PasswordHash;
            byte[] salt = resultado.PasswordSalt;

            // Arma el query base
            string query = @"UPDATE Users 
                     SET Name = @Name,
                         PasswordHash = @PasswordHash,
                         PasswordSalt = @PasswordSalt,
                         Email = @Email";

            if (rol == "admin")
                query += ", Role = @Role";

            query += " WHERE Id = @Id;"; 
            using (var conn = _conexion.Abrir_Conexion())
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = Guid.Parse(ID);
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 256).Value = usuario.Email;
                cmd.Parameters.Add("@PasswordHash", SqlDbType.VarBinary, -1).Value = hash;
                cmd.Parameters.Add("@PasswordSalt", SqlDbType.VarBinary, -1).Value = salt;
                cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 150).Value = usuario.Name;
                if (rol == "admin")
                    cmd.Parameters.Add("@Role", SqlDbType.NVarChar, 20).Value = usuario.Role;

                cmd.ExecuteNonQuery();
            }
        }
        public string GenerarToken(string email, IConfiguration config, string rol)
        {
            var jwtSettings = config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, rol),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpireMinutes"])),
                signingCredentials: credenciales
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }    
}
