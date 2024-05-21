using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CATALOGO_DE_PRODUCTOS.Data;
using CATALOGO_DE_PRODUCTOS.Models;
using Microsoft.Extensions.Hosting;
using CATALOGO_DE_PRODUCTOS.ValidarAcceso;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Protocol;

namespace CATALOGO_DE_PRODUCTOS.Controllers
{
    [ValidarLogin]
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _hostEnvironment;

        public ProductoController(ApplicationDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Producto
        public async Task<IActionResult> Index(string filtro)
        {
            IQueryable<Producto> applicationDbContext = _context.Producto.Include(p => p.Categoria).
                OrderByDescending(p=>p.Precio);

            if (!string.IsNullOrEmpty(filtro))
            {
                applicationDbContext = applicationDbContext.Where(p => p.Categoria.Nombre.Contains(filtro)).
                    OrderByDescending(p=>p.Precio);
            }

            ViewBag.Categoria = new SelectList(_context.Categoria, "Id", "Nombre");
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Producto/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre");
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile Imagen)
        {
            try
            {
                if (!string.IsNullOrEmpty(producto.IdCategoria.ToString()))
                {
                    if (Imagen != null && Imagen.Length > 0)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagen", Imagen.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Imagen.CopyToAsync(stream);
                        }
                        producto.Imagen = "/Imagen/" + Imagen.FileName;
                    }
                    producto.FechaCreacion = DateTime.Now;
                    _context.Add(producto);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre", producto.IdCategoria);
                return View(producto);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.ImagenActual = producto.Imagen;
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre", producto.IdCategoria);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto, IFormFile Imagen, string nombreArchivo)
        {
            try
            {
                if (id != producto.Id)
                {
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(producto.IdCategoria.ToString()))
                {
                    try
                    {
                        if (Imagen != null && Imagen.Length > 0)
                        {
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Imagen", Imagen.FileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await Imagen.CopyToAsync(stream);
                            }
                            producto.Imagen = "/Imagen/" + Imagen.FileName;
                        }
                        if (string.IsNullOrEmpty(producto.Imagen))
                        {
                            producto.Imagen = nombreArchivo;
                        }
                        producto.FechaCreacion = DateTime.Now;
                        _context.Update(producto);
                        await _context.SaveChangesAsync();               
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProductoExists(producto.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                ViewData["IdCategoria"] = new SelectList(_context.Categoria, "Id", "Nombre", producto.IdCategoria);
                return View(producto);
            }
            catch (Exception ex)
            {
                return View();
                throw ex;          
            }
        }

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Producto == null)
            {
                return NotFound();
            }

            var producto = await _context.Producto
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Producto == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Producto'  is null.");
            }
            var producto = await _context.Producto.FindAsync(id);
            if (producto != null)
            {
                _context.Producto.Remove(producto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductoExists(int id)
        {
            return (_context.Producto?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
