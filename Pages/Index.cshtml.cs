using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

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


        public void OnGet()
        {
            var task = GetArticles();
            task.Wait();

            ViewData["articles"] = task.Result;
        }

        private async Task<List<Articles>> GetArticles()
        {
            var articles = (from a in _db.article select a).ToList();
            
            for (int i=0; i<articles.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(articles[i].Owner);
                
                articles[i].Owner = user.UserName;
            }

            return articles;
        }
    }
}
