using AnniesShop.Models;
using AnniesShop.Models.ViewModels;

namespace AnniesShop.Services
{
    public interface IProductoService
    {
        Producto GetProducto(int id);

        Task<List<Producto>> GetProductosDestacados();

        Task<ProductosPaginadosViewModel> GetProductosPaginados(int? categoriaId, string? busqueda, int pagina, int productosPorPagina);
    }
}