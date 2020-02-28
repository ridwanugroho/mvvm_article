using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Article.Model;
using Article.Data;

namespace Article.Pages
{
    public class RoleManagerModel : PageModel
    {
        private ApplicationDbContext _db;

        private readonly ILogger<RoleManagerModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        private RoleManager<IdentityRole> _roleManager;

        public RoleManagerModel(ILogger<RoleManagerModel> logger, ApplicationDbContext db,
                        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> OnGet()
        {
            ViewData["usersAndRoles"] = await getAllUserRole();

            return Page();
        }

        public void OnPost()
        {
        }

        private async Task<List<UserRoleDetail>> getAllUserRole()
        {
            var user = from u in _userManager.Users select u;
            
            var list = new List<UserRoleDetail>();

            foreach(var u in user)
            {
                var role = await _userManager.GetRolesAsync(u);
                var _role = "null";
                if(role.Count() > 0)
                    _role = role.First();

                list.Add(new UserRoleDetail
                {
                    UserId = u.Id,
                    Username = u.UserName,
                    role = _role
                });
            }

            return list;
        }

        public async Task<IActionResult> OnGetCreateRole(string role)
        {
            if(ModelState.IsValid)
            {
                IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(role));
                if(result.Succeeded)
                    return Content("OK");
                
                else
                    return Content("Bad");
            }

            return Content("?");
        }

        public async Task<IActionResult> OnGetSetRole(string userId, string role)
        {

            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            if(!result.Succeeded)
                return Content("Bad");

            return Redirect("/RoleManager");
        }

        public async Task<IActionResult> removeFromPrevRole(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);
            if(!result.Succeeded)
                return Content("Bad");

            return Redirect("/RoleManager");
        }
    }
}