using AnniesShop.Data;
using AnniesShop.Models;
using Microsoft.EntityFrameworkCore;

namespace AnniesShop.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categoria>> GetCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }
    }
}