using Backend;
using Backend.Estructura;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class API: ControllerBase
{
    private readonly Usuarios_BD BaseDatos;
    private readonly Validaciones Validacion;
    private readonly IConfiguration _config;

    public API(Usuarios_BD baseDatos, Validaciones validacion, IConfiguration config)
    {
        BaseDatos = baseDatos;
        Validacion = validacion;
        _config = config;
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] Usuarios usuario)
    {
        try
        {
            var resultado = Validacion.Registrar_Usuario(BaseDatos, usuario);
            return Ok(resultado);
        }
        catch (Exception ex){
            return StatusCode(500, $"❌ Error al listar usuarios: {ex.Message}");
        }
    }

    [HttpPost("Login")]
    public IActionResult Login([FromBody] Login usuario)
    {
        try
        {
            var resultado = Validacion.Logear_Usuario(BaseDatos, usuario, _config);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al listar usuarios: {ex.Message}");
        }
    }

    [HttpGet("Admin/ListarUsuarios")]

    [Authorize(Roles = "admin")]
    public IActionResult ListarUsuarios()
    {
        try
        {
            var usuarios = Validacion.Listar_Usuarios(BaseDatos);
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al listar usuarios: {ex.Message}");
        }
    }
    [HttpDelete("Admin/Eliminar_Usuario/{id}")]
    [Authorize(Roles = "admin")]
    public IActionResult Eliminar_Usuario(string id)
    {
        try
        {
            var resultado = Validacion.Eliminar_Usuario(BaseDatos, id);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al eliminar usuario: {ex.Message}");
        }
    }
    [HttpPost("Admin/AñadirUsuario")]
    [Authorize(Roles = "admin")]
    public IActionResult Añadir_Usuario_Administrador([FromBody] Usuario_Entrada usuario)
    {
        try
        {
            var resultado = Validacion.Añadir_Usuario_Admin(BaseDatos, usuario);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al añadir usuario: {ex.Message}");
        }
    }

    [HttpGet("Loged/VerUsuario/{id}")]
    [Authorize(Roles = "admin, user")]

    public IActionResult Ver_Usuario(string id)
    {
        try
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var resultado = Validacion.VerUsuario(BaseDatos, id, rol.ToString(), email.ToString());
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al ver usuario: {ex.Message}");
        }
    }

    [HttpPut("Loged/EditarUsuario/{id}")]
    [Authorize(Roles = "admin, user")]

    public IActionResult Editar_Usuario(string id, [FromBody] Usuario_Entrada usuario)
    {
        try
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var resultado = Validacion.EditarUsuario(BaseDatos, id, rol.ToString(), email.ToString(), usuario);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"❌ Error al editar usuario: {ex.Message}");
        }
    }
}
