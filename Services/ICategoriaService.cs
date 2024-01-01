using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AnniesShop.Models;

namespace AnniesShop.Services
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> GetCategorias();
    }
}