using AnniesShop.Models;

namespace AnniesShop.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategorias();
    }
}