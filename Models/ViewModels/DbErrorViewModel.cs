using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnniesShop.Models.ViewModels
{
    public class DbErrorViewModel
    {
        public string ErrorMessage { get; set; } = null!;

        public string Details { get; set; } = null!;
    }
}