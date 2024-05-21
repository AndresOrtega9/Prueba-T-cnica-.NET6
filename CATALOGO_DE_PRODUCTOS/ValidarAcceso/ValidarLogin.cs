using CATALOGO_DE_PRODUCTOS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NuGet.Protocol;

namespace CATALOGO_DE_PRODUCTOS.ValidarAcceso
{
    public class ValidarLogin : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext.HttpContext.Session.GetString("UsuarioId") == null)
            {
                filterContext.Result = new RedirectResult("~/Acceso/Login");
            }
            else
            {
                filterContext.HttpContext.Response.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
                filterContext.HttpContext.Response.Headers.Add("Pragma", "no-cache");
                filterContext.HttpContext.Response.Headers.Add("Expires", "0");
            }
            base.OnActionExecuting(filterContext);
        }

    }
}
