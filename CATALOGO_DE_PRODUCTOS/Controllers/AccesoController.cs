using CATALOGO_DE_PRODUCTOS.Data;
using CATALOGO_DE_PRODUCTOS.Models;
using Microsoft.AspNetCore.Mvc;

namespace CATALOGO_DE_PRODUCTOS.Controllers
{
    public class AccesoController : Controller
    {
        private readonly ApplicationDbContext _applicationDbContext;
        public AccesoController(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            try
            {
                var passwordEncriptada = Utilidades.Utilidades.EncriptarPassword(password);
                var usuario = _applicationDbContext.Usuario.FirstOrDefault(u => u.Username == username && u.Password == passwordEncriptada);
                if (usuario != null)
                {
                    HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                    return RedirectToAction("Index", "Producto");
                }
                else
                {
                    ViewData["Msj"] = "Usuario no encontrado, verifique sus credenciales";
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
                throw ex;

            }
        }
        public ActionResult CerrarSesion()
        {
            try
            {
                HttpContext.Session.Remove("UsuarioId");
                return RedirectToAction("Login", "Acceso");
            }
            catch(Exception ex)
            {
                throw ex;
                return null;
            }         
        }
    }
}
