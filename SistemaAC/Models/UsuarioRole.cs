using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAC.Models
{
    public class UsuarioRole
    {
        public List<SelectListItem> usuarioRoles;
        //Constructor
        public UsuarioRole()
        {
            usuarioRoles = new List<SelectListItem>();
        }

        //Methods

        //Devolver Rol de Usuario
        public async Task<List<SelectListItem>> GetRole(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,string Id)
        {
            usuarioRoles = new List<SelectListItem>();
            string rol;
            var usuario = await userManager.FindByIdAsync(Id);
            var roles = await userManager.GetRolesAsync(usuario);

            if (roles.Count == 0)
            {
                usuarioRoles.Add(new SelectListItem() {
                    Value = "null",
                    Text = "No Role"
                });
            }
            else
            {
                rol = Convert.ToString(roles[0]);
                var rolesId = roleManager.Roles.Where(m => m.Name == rol);
                foreach (var item in rolesId)
                {
                    usuarioRoles.Add(new SelectListItem()
                    {
                        Value = item.Id,
                        Text = item.Name
                    });
                }
            }
            return usuarioRoles;
            
        }
        
        //Devolver Lista de Roles Disponibles
        public async Task<List<SelectListItem>> GetRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = roleManager.Roles.ToList();
            foreach (var item in roles)
            {
                usuarioRoles.Add(new SelectListItem()
                {
                    Value = item.Id,
                    Text = item.Name
                });
            }
            return usuarioRoles;
        }
    }
}
