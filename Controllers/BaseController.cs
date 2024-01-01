using System.Data.Common;
using System.Diagnostics;
using AnniesShop.Data;
using AnniesShop.Models;
using AnniesShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace AnniesShop.Controllers
{
    
    public class BaseController : Controller
    {
        public readonly ApplicationDbContext _context;

        public BaseController(ApplicationDbContext context)
        {
            _context = context;
        }

        public override ViewResult View(string? viewName, object? model)
        {
            ViewBag.NumeroProductos=GetCarritoCount();
            return base.View(viewName, model);
        }

        protected int GetCarritoCount()
        {
            int count = 0;
            
            string? carritoJson=Request.Cookies["carrito"];

            if (!string.IsNullOrEmpty(carritoJson))
            {
                var carrito = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(
                    carritoJson
                );
                if (carrito !=null)
                {
                    count = carrito.Count;
                }
            }

            return count;
        }

        protected IActionResult HandleError(Exception e)
        {
            return View(
                "Error",new ErrorViewModel{
                    RequestId=Activity.Current?.Id ?? HttpContext.TraceIdentifier
                }
            );
        }

        protected IActionResult HandleDbError(DbException dbException)
        {
            var viewModel = new DbErrorViewModel{
                ErrorMessage="Error de base de datos",
                Details=dbException.Message
            };
            return View("DbError", viewModel);
        }

        protected IActionResult HandleDbUpdateError(DbUpdateException dbUpdateException)
        {
            var viewModel = new DbErrorViewModel{
                ErrorMessage="Error de actualizacion de datos",
                Details=dbUpdateException.Message
            };
            return View("DbError", viewModel);
        }
    }
}