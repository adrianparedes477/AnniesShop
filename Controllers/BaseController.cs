using AnniesShop.Data;
using AnniesShop.Models;
using Microsoft.AspNetCore.Mvc;
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
    }
}