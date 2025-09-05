using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Estructura
{
    public class Validaciones
    {

        public Resultado Registrar_Usuario(Usuarios_BD BaseDatos, Usuarios usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Name) || string.IsNullOrWhiteSpace(usuario.Password))
            {
                return new Resultado { Success = false, Mensaje = "El email, nombre y contraseña son obligatorios" };
            }

            if (BaseDatos.ObtenerUsuarioPorEmail(usuario.Email) != false)
            {
                return new Resultado { Success = false, Mensaje = "Este email ya esta registrado" };
            }
            try
            {
                BaseDatos.Registrar_Usuario(usuario);
                return new Resultado { Success = true, Mensaje = "Usuario agregado correctamente" };
            }
            catch (Exception ex)
            {
                return new Resultado { Success = false, Mensaje = $"Error al registrar usuario: {ex.Message}" };
            }
        }

        public Resultado Logear_Usuario(Usuarios_BD BaseDatos, Login usuario, IConfiguration _config)
        {
            if (!BaseDatos.ObtenerUsuarioPorEmail(usuario.Email))
            {
                return new Resultado { Success = false, Mensaje = "Este email no está registrado" };
            }
            if (!BaseDatos.Verificar_Contraseña(usuario.Email, usuario.Password))
            {
                return new Resultado { Success = false, Mensaje = "Contraseña inválida" };
            }
            return new Resultado
            {
                Success = true,
                Mensaje = "Inicio de sesión correcto",
                Datos = BaseDatos.GenerarToken(usuario.Email, _config, BaseDatos.Tipo_Usuario(usuario.Email))
            };
        }
        public Resultado Eliminar_Usuario(Usuarios_BD BaseDatos, string ID)
        {
            if (!BaseDatos.VerificarID(ID))
            {
                return new Resultado { Success = false, Mensaje = "Este ID no corresponde a ningun usuario." };
            }
            ;
            try
            {
                BaseDatos.Eliminar_Usuario(ID);
                return new Resultado { Success = true, Mensaje = "Usuario eliminado correctamente." };
            }
            catch (Exception ex)
            {
                return new Resultado { Success = false, Mensaje = $"Error al eliminar usuario: {ex.Message}" };
            }
        }

        public Resultado Añadir_Usuario_Admin(Usuarios_BD BaseDatos, Usuario_Entrada usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Name) || string.IsNullOrWhiteSpace(usuario.Role) || string.IsNullOrWhiteSpace(usuario.Password))
            {
                return new Resultado { Success = false, Mensaje = "El email, nombre, contraseña y rol son obligatorios" };
            }

            if (BaseDatos.ObtenerUsuarioPorEmail(usuario.Email) != false)
            {
                return new Resultado { Success = false, Mensaje = "Este email ya esta registrado" };
            }

            if (BaseDatos.VerificarRol(usuario.Role) == false)
            {
                return new Resultado { Success = false, Mensaje = "Rol invalido" };
            }
            try
            {
                BaseDatos.Añadir_Usuario_Admin(usuario);
                return new Resultado { Success = true, Mensaje = "Usuario agregado correctamente ✅" };
            }
            catch (Exception ex)
            {
                return new Resultado { Success = false, Mensaje = $"Error al registrar usuario: {ex.Message}" };
            }
        }

        public Resultado VerUsuario(Usuarios_BD BaseDatos, string ID, string Rol, string email)
        {
            if (string.IsNullOrWhiteSpace(ID))
                return new Resultado { Success = false, Mensaje = "El ID es obligatorio" };
            if (!BaseDatos.VerificarID(ID))
                return new Resultado { Success = false, Mensaje = "Este ID no corresponde a ningun usuario." };
            var usuario = new Mostrar_Usuario();

            try
            {
                if (Rol == "admin")
                {
                    BaseDatos.Ver_Usuario(ID, usuario);
                    return new Resultado
                    {
                        Success = true,
                        Mensaje = "Usuario mostrado correctamente",
                        Datos = usuario
                    };
                }

                if (BaseDatos.ID_Correspiente(ID, email))
                {
                    BaseDatos.Ver_Usuario(ID, usuario);
                    return new Resultado
                    {
                        Success = true,
                        Mensaje = "Usuario mostrado correctamente",
                        Datos = usuario
                    };
                }

                return new Resultado { Success = false, Mensaje = "Este usuario no puede ver otros perfiles" };
            }
            catch (Exception ex)
            {
                return new Resultado
                {
                    Success = false,
                    Mensaje = $"Error al ver usuario: {ex.Message}"
                };
            }
        }
        public Resultado EditarUsuario(Usuarios_BD BaseDatos, string ID, string Rol, string email, Usuario_Entrada usuario)
        {
            if (string.IsNullOrWhiteSpace(ID))
                return new Resultado { Success = false, Mensaje = "El ID es obligatorio" };
            if (!BaseDatos.VerificarID(ID))
                return new Resultado { Success = false, Mensaje = "Este ID no corresponde a ningun usuario." };
            try
            {
                if (Rol == "admin")
                {
                    if (string.IsNullOrWhiteSpace(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Name) || string.IsNullOrWhiteSpace(usuario.Password) || string.IsNullOrWhiteSpace(usuario.Role))
                    {
                        return new Resultado { Success = false, Mensaje = "El email, nombre, contraseña, contraseña y rol son obligatorios" };
                    }
                    BaseDatos.Editar_Usuario(ID, usuario, Rol);
                    return new Resultado
                    {
                        Success = true,
                        Mensaje = "Usuario editado correctamente",
                    };
                }
                else
                {
                    if (BaseDatos.ID_Correspiente(ID, email))
                    {
                        if (!string.IsNullOrWhiteSpace(usuario.Role))
                        {
                            return new Resultado { Success = false, Mensaje = "Un usuario no puede editar su rol" };
                        }

                        BaseDatos.Editar_Usuario(ID, usuario, Rol);
                        return new Resultado
                        {
                            Success = true,
                            Mensaje = "Usuario editado correctamente",
                        };
                    }
                    return new Resultado { Success = false, Mensaje = "Este usuario no editar otros perfiles" };
                }
            }
            catch (Exception ex)
            {
                return new Resultado
                {
                    Success = false,
                    Mensaje = $"Error al editar usuario: {ex.Message}"
                };
            }
        }

        public Resultado Listar_Usuarios (Usuarios_BD Base_Datos)
        {
           return new Resultado { Success = true, Mensaje = "Usuarios", Datos = Base_Datos.ListarUsuarios() };
        }
    }
}
