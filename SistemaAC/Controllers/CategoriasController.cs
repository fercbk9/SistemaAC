using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAC.Data;
using SistemaAC.Models;
using SistemaAC.ModelsClass;

namespace SistemaAC.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private CategoriaModels categoriaModels;
        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
            categoriaModels = new CategoriaModels(_context);
            
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categoria.ToListAsync());
        }

                

        public List<object[]> filtrarDatos(int numPagina, string valor,string order)
        {
            return categoriaModels.filtrarDatos(numPagina, valor, order);

        }

        public List<Categoria> getCategoria(int id)
        {
            return categoriaModels.getCategorias(id);
        }

        public List<IdentityError> CreateCategoria(Categoria categoria)
        {
            return categoriaModels.CreateCategoria(categoria);
        }

        public List<IdentityError> editarCategoria(Categoria categoria, int funcion/*, int id*/)
        {
            return categoriaModels.editarCategoria(categoria, funcion/*,id*/);
        }

        private bool CategoriaExists(int id)
        {
            return _context.Categoria.Any(e => e.CategoriaID == id);
        }
    }
}
