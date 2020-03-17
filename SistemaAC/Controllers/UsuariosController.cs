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

namespace SistemaAC.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        UserManager<ApplicationUser> _userManager;
        RoleManager<IdentityRole> _roleManager;
        UsuarioRole _usuarioRole;

        public List<SelectListItem> usuarioRole;

        public UsuariosController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _usuarioRole = new UsuarioRole();
            usuarioRole = new List<SelectListItem>();
        }


        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var ID = "";
            List<Usuario> usuario = new List<Usuario>();
            var appUsuario = await _context.ApplicationUser.ToListAsync();
            foreach (var item in appUsuario)
            {
                ID = item.Id;
                usuarioRole = await _usuarioRole.GetRole(_userManager, _roleManager, ID);
                usuario.Add(new Usuario()
                {
                    Id=item.Id,
                    UserName = item.UserName,
                    PhoneNumber = item.PhoneNumber,
                    Email = item.Email,
                    Role = usuarioRole[0].Text
                });
            }
            return View(usuario.ToList());
            //return View(await _context.ApplicationUser.ToListAsync());
        }

        //Get Usuario
        public async Task<List<Usuario>> GetUsuario(string id)
        {
            List<Usuario> usuario = new List<Usuario>();
            var appUsuario = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
            usuarioRole = await _usuarioRole.GetRole(_userManager, _roleManager, id);
            usuario.Add(new Usuario()
            {
                Id = appUsuario.Id,
                UserName = appUsuario.UserName,
                PhoneNumber = appUsuario.PhoneNumber,
                Email = appUsuario.Email,
                Role = usuarioRole[0].Text,
                RoleId = usuarioRole[0].Value,
                AccessFailedCount = appUsuario.AccessFailedCount,
                ConcurrencyStamp = appUsuario.ConcurrencyStamp,
                EmailConfirmed = appUsuario.EmailConfirmed,
                LockoutEnabled = appUsuario.LockoutEnabled,
                LockoutEnd = appUsuario.LockoutEnd,
                NormalizedEmail = appUsuario.NormalizedEmail,
                NormalizedUserName = appUsuario.NormalizedUserName,
                PasswordHash = appUsuario.PasswordHash,
                PhoneNumberConfirmed = appUsuario.PhoneNumberConfirmed,
                SecurityStamp = appUsuario.SecurityStamp,
                TwoFactorEnabled = appUsuario.TwoFactorEnabled
            });
            return usuario;

        }
        
        //Get Roles
        public async Task<List<SelectListItem>> getRoles()
        {
            List<SelectListItem> rolesList = new List<SelectListItem>();
            rolesList = await _usuarioRole.GetRoles(_roleManager);
            return rolesList;
        }
        //Edit Usuario
        public async Task<string> EditUsuario(string id, string userName, string email, string phoneNumber, int accessFailedCount, string concurrencyStamp,
            bool emailConfirmed, bool lockoutEnabled, DateTimeOffset lockoutEnd, string normalizedEmail, string normalizedUserName, string passwordHash,
            bool phoneNumberConfirmed, string securityStamp, bool twoFactorEnabled, string selectRole,ApplicationUser appUser)
        {
            var resp = "";
            try
            {
                appUser = new ApplicationUser
                {
                    Id = id,
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    EmailConfirmed = emailConfirmed,
                    LockoutEnabled = lockoutEnabled,
                    LockoutEnd = lockoutEnd,
                    NormalizedEmail = normalizedEmail,
                    NormalizedUserName = normalizedUserName,
                    PasswordHash = passwordHash,
                    PhoneNumberConfirmed = phoneNumberConfirmed,
                    SecurityStamp = securityStamp,
                    TwoFactorEnabled = twoFactorEnabled,
                    AccessFailedCount = accessFailedCount,
                    ConcurrencyStamp = concurrencyStamp
                    
                };
                _context.Update(appUser);
                await _context.SaveChangesAsync();
                var usuario = await _userManager.FindByIdAsync(id);
                usuarioRole = await _usuarioRole.GetRole(_userManager, _roleManager, id);

                if (usuarioRole[0].Text != "No Role")
                {
                    await _userManager.RemoveFromRoleAsync(usuario, usuarioRole[0].Text);
                }
                if(selectRole == "No Role")
                {
                    selectRole = "Usuario";
                }
                var resultado = await _userManager.AddToRoleAsync(usuario, selectRole);

                resp = "Save";
            }
            catch (Exception)
            {

                resp = "No Save";
            }
            return resp;
        }

        public async Task<String> DeleteUsuario(string id)
        {
            var resp = "";
            try
            {
                var applicationUser = await _context.ApplicationUser.SingleOrDefaultAsync(m => m.Id == id);
                _context.ApplicationUser.Remove(applicationUser);
                await _context.SaveChangesAsync();
                resp = "Delete";
            }
            catch (Exception)
            {

                resp = "No Delete";
            }
            return resp;
        }

        public async Task<Boolean> CreateUsuario(string selectRole, ApplicationUser usuario)
        {
            var resp = false;
            ApplicationUser applicationUser = new ApplicationUser
            {
                UserName = usuario.Email,
                Email = usuario.Email,
                PhoneNumber = usuario.PhoneNumber
            };
            
            var result = await _userManager.CreateAsync(applicationUser, usuario.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(applicationUser, selectRole);
                resp = true;
            }
            else
            {
                resp = false;
            }
            return resp;
        }

        private bool ApplicationUserExists(string id)
        {
            return _context.ApplicationUser.Any(e => e.Id == id);
        }


    }
}
