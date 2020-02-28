using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Article.Model;
using Article.Data;

namespace Article.Pages
{
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _db;

        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext db,
                        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult OnGet()
        {
            if(validateUser(_userManager.GetUserId(User), "admin"))
            {
                return Redirect("/admin/article");
            }

            var task = GetArticles();
            task.Wait();

            ViewData["articles"] = task.Result;

            return Page();
        }

        private async Task<List<Articles>> GetArticles()
        {
            var articles = (from a in _db.article select a).ToList();
            
            for (int i=0; i<articles.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(articles[i].Owner);
                
                // articles[i].Owner = user.UserName;
            }

            return articles;
        }

        private bool validateUser(string id, string role)
        {
            var emptyUser = "00000000-0000-0000-0000-000000000000";
            
            if(id == emptyUser || string.IsNullOrEmpty(id))
                return false;
            
            var user = (from r in _db.role where r.UserId == id select r).First();

            switch (role)
            {
                case "user":
                if(user.Role == 1)
                    return true;
                break;

                case "author":
                if(user.Role == 2)
                    return true;
                break;

                case "admin":
                if(user.Role == 3)
                    return true;
                break;
            }

            return false;

        }
    }
}