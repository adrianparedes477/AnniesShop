
using AnniesShop.Data;
using AnniesShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnniesShop.Controllers
{
    public class PerfilController : BaseController
    {
        public PerfilController(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
                return NotFound();

            var usuario = await _context.Usuarios
            .Include(u => u.Direcciones)
            .FirstOrDefaultAsync(u => u.UsuarioId == Id);

            if (usuario == null)
                return NotFound();

            return View(usuario);
        }

        public IActionResult AgregarDireccion(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarDireccion(Direccion direccion, int Id)
        {
            try
            {
                var usuario = await _context.Usuarios
                                    .FirstOrDefaultAsync(u => u.UsuarioId == Id);
                if (usuario != null)
                    direccion.Usuario = usuario;

                _context.Add(direccion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new { Id });
            }
            catch (System.Exception)
            {
                return View(direccion);
            }


        }
    }
}