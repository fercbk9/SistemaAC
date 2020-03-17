using Microsoft.AspNetCore.Identity;
using SistemaAC.Data;
using SistemaAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAC.ModelsClass
{
    public class CategoriaModels
    {
        private ApplicationDbContext context;
        private Boolean estados;
        public CategoriaModels(ApplicationDbContext context)
        {
            this.context = context;
            //filtrarDatos(1, "J");
        }
        public List<IdentityError> CreateCategoria(Categoria categoria)
        {
            var errorList = new List<IdentityError>();
            var _categoria = new Categoria
            {
                Nombre = categoria.Nombre,
                Descripcion = categoria.Descripcion,
                Estado = categoria.Estado
            };
            this.context.Add(categoria);
            this.context.SaveChanges();
            errorList.Add(new IdentityError
            {
                Code = "Save",
                Description = "Save"
            });
            return errorList;
        }
        public List<Object[]> filtrarDatos(int numPegina,string valor, string order)
        {
            int count = 0, cant, numRegistros = 0, inicio = 0, reg_por_pagina = 2;
            int can_paginas, pagina;
            string dataFilter = "", paginador = "", Estado = null;
            List<Object[]> data = new List<object[]>();
            IEnumerable<Categoria> query;
            List<Categoria> categorias = null;
            switch (order)
            {
                case "nombre":
                    categorias = context.Categoria.OrderBy(c => c.Nombre).ToList();
                    break;
                case "descripcion":
                    categorias = context.Categoria.OrderBy(c => c.Descripcion).ToList();
                    break;
                case "estado":
                    categorias = context.Categoria.OrderBy(c => c.Estado).ToList();
                    break;
            }
            
            numRegistros = categorias.Count;
            inicio = (numPegina - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);
            if (numRegistros % reg_por_pagina > 0)
            {
                can_paginas ++;
            }
            if (valor == "null")
            {
                query = categorias.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                query = categorias.Where(c => c.Nombre.StartsWith(valor) || c.Descripcion.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);
            }
            cant = query.Count();
            foreach (var item in query)
            {
                if (item.Estado)
                {
                    Estado = "<a data-toggle='modal' data-target='#ModalEstado' onclick='editarEstado(" + item.CategoriaID + "," + 0 + ")' class='btn btn-success'>Activo</a>";
                }
                else
                {
                    Estado = "<a data-toggle='modal' data-target='#ModalEstado' onclick='editarEstado(" + item.CategoriaID + "," + 0 + ")' class='btn btn-danger'>No Activo</a>";
                }
                dataFilter += "<tr>" +
                    "<td>" + item.Nombre + "</td>" +
                    "<td>" + item.Descripcion + "</td>" +
                    "<td>" + Estado + "</td>" +
                    "<td>" +
                    "<a data-toggle='modal' data-target='#modalAgregarCategoria' onclick='editarEstado(" + item.CategoriaID + "," + 1 + ")' class='btn btn-success'>Editar</a>" +
                    "</td>" +
                    "</tr>";
            }
            if (valor == "null")
            {
                if (numPegina > 1)
                {
                    pagina = numPegina - 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarDatos(" + 1 + ',' + '"' + order + '"' + ")'> << </a>";
                    paginador += "<a class='btn btn-default' onclick='filtrarDatos(" + pagina + ',' + '"' + order + '"' + ")'> < </a>";
                }
                if (1 < can_paginas)
                {
                    
                    paginador += "<strong class='btn btn-success'> "+ numPegina +" de "+ can_paginas +"</strong>";
                    
                }
                if (numPegina < can_paginas)
                {
                    pagina = numPegina + 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarDatos(" + pagina + ',' +'"' + order + '"' +")'> > </a>";
                    paginador += "<a class='btn btn-default' onclick='filtrarDatos(" + can_paginas + ',' + '"' + order + '"' + ")'> >> </a>";
                }
            }
            object[] dataObj = { dataFilter, paginador };
            data.Add(dataObj);
            return data;
        }
        public List<Categoria> getCategorias(int id)
        {
            return context.Categoria.Where(c => c.CategoriaID == id).ToList();
        }
        public List<IdentityError> editarCategoria(Categoria categoria, int funcion/*,int id*/)
        {
            var errorList = new List<IdentityError>();
            switch (funcion)
            {
                case 0:
                    if (categoria.Estado)
                    {
                        estados = false;
                        categoria.Estado = false;
                    }
                    else
                    {
                        estados = true;
                        categoria.Estado = true;
                    }

                    /*Categoria c = new Categoria
                    {
                        CategoriaID = id,
                        Nombre = categoria.Nombre,
                        Descripcion = categoria.Descripcion,
                        Estado = estados
                    };*/
                    context.Categoria.Update(categoria);
                    context.SaveChanges();

                    break;
                default:
                    context.Categoria.Update(categoria);
                    context.SaveChanges();
                    break;
            }
            errorList.Add(new IdentityError {
                Code = "Save",
                Description = "Save"
            });
        return errorList;
        }
    }
}
