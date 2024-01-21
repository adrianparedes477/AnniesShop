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
            ViewBag.NumeroProductos = GetCarritoCount();
            return base.View(viewName, model);
        }

        protected int GetCarritoCount()
        {
            int count = 0;
            try
            {
                string carritoJson = Request.Cookies["carrito"];
                if (!string.IsNullOrEmpty(carritoJson))
                {
                    var carrito = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(carritoJson);
                    if (carrito != null)
                    {
                        count = carrito.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                // Manejar la excepción o registrarla según sea necesario
                // Log.Error(ex, "Error al obtener el carrito count");
            }

            return count;
        }

        public async Task<CarritoViewModel> AgregarProductoAlCarrito(int productoId, int cantidad)
        {
            try
            {
                var producto = await _context.Productos.FindAsync(productoId);

                if (producto != null && cantidad > 0)
                {
                    var carritoViewModel = await GetCarritoViewModelAsync();

                    var carritoItem = carritoViewModel.Items.FirstOrDefault(item => item.ProductoId == productoId);

                    if (carritoItem != null)
                        carritoItem.Cantidad += cantidad;
                    else
                    {
                        carritoViewModel.Items.Add(new CarritoItemViewModel
                        {
                            ProductoId = producto.ProductoId,
                            Nombre = producto.Nombre,
                            Precio = producto.Precio,
                            Cantidad = cantidad
                        });
                    }
                    carritoViewModel.Total = carritoViewModel.Items.Sum(item => item.Cantidad * item.Precio);

                    await UpdateCarritoViewModelAsync(carritoViewModel);
                    return carritoViewModel;
                }

                return new CarritoViewModel();
            }
            catch (Exception ex)
            {
                // Manejar la excepción o registrarla según sea necesario
                // Log.Error(ex, "Error al agregar producto al carrito");
                return new CarritoViewModel();
            }
        }

        private async Task UpdateCarritoViewModelAsync(CarritoViewModel carritoViewModel)
        {
            try
            {
                var productoIds = carritoViewModel.Items
                    .Select(item => new ProductoIdAndCantidad
                    {
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad
                    })
                    .ToList();
                var carritoJson = await Task.Run(() => JsonConvert.SerializeObject(productoIds));
                Response.Cookies.Append(
                    "carrito",
                    carritoJson,
                    new CookieOptions { Expires = DateTimeOffset.Now.AddDays(7) }
                );
            }
            catch (Exception ex)
            {
                
                // Log.Error(ex, "Error al actualizar el carrito");
            }
        }

        public async Task<CarritoViewModel> GetCarritoViewModelAsync()
        {
            try
            {
                var carritoJson = Request.Cookies["carrito"];

                if (string.IsNullOrEmpty(carritoJson))
                    return new CarritoViewModel();

                var productoIdsAndCantidades = JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(carritoJson);
                var carritoViewModel = new CarritoViewModel();

                if (productoIdsAndCantidades != null)
                {
                    foreach (var item in productoIdsAndCantidades)
                    {
                        var producto = await _context.Productos.FindAsync(item.ProductoId);
                        if (producto != null)
                        {
                            carritoViewModel.Items.Add(new CarritoItemViewModel
                            {
                                ProductoId = producto.ProductoId,
                                Nombre = producto.Nombre,
                                Precio = producto.Precio,
                                Cantidad = item.Cantidad
                            });
                        }
                    }
                }
                carritoViewModel.Total = carritoViewModel.Items.Sum(item => item.Subtotal);
                return carritoViewModel;
            }
            catch (Exception ex)
            {
                // Manejar la excepción o registrarla según sea necesario
                // Log.Error(ex, "Error al obtener el carrito ViewModel");
                return new CarritoViewModel();
            }
        }

        protected IActionResult HandleError(Exception e)
        {
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        protected IActionResult HandleDbError(DbException dbException)
        {
            var viewModel = new DbErrorViewModel
            {
                ErrorMessage = "Error de base de datos",
                Details = dbException.Message
            };
            return View("DbError", viewModel);
        }

        protected IActionResult HandleDbUpdateError(DbUpdateException dbUpdateException)
        {
            var viewModel = new DbErrorViewModel
            {
                ErrorMessage = "Error de actualizacion de datos",
                Details = dbUpdateException.Message
            };
            return View("DbError", viewModel);
        }
    }

}