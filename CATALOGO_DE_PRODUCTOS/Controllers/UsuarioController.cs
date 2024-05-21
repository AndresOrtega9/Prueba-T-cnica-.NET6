using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CATALOGO_DE_PRODUCTOS.Data;
using CATALOGO_DE_PRODUCTOS.Models;

namespace CATALOGO_DE_PRODUCTOS.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password,ConfirmarPassword")] Usuario usuario)
        {
            try
            {
                if (usuario.Password == usuario.ConfirmarPassword)
                {
                    var consultaExistente = _context.Usuario.Any(u => u.Username.Equals(usuario.Username));
                    if (!consultaExistente)
                    {
                        if (ModelState.IsValid)
                        {
                            usuario.Password = Utilidades.Utilidades.EncriptarPassword(usuario.Password);
                            _context.Add(usuario);
                            await _context.SaveChangesAsync();
                        }
                        ViewData["Msj"] = "Usuario creado con exito";
                        return View(usuario);
                    }
                    else
                    {
                        ViewData["Msj"] = "Usuario ya existe";
                    }
                }

                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
