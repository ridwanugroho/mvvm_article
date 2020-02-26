using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

using Article.Data;
using Article.Model;

namespace Article.Areas.Admin.Pages
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

        public void OnGet(string delete)
        {
            var articles = GetArticles(_userManager.GetUserId(User));
            ViewData["articles"] = articles;

            foreach(var a in articles)
                Console.WriteLine(a.Titile);

            if(validateStr(delete))
            {
                deleteArticle(delete);
            }
        }

        private void deleteArticle(string deleteId)
        {
            var deleteArticle = _db.article.Find(Guid.Parse(deleteId));
            deleteArticle.Status = "deleted";
            deleteArticle.Deleted_at = DateTime.Now;
            _db.SaveChanges();
        }

        private List<Articles> GetArticles(string ownerId)
        {
            var user = User.Identity.Name;
            var articles = (from a in _db.article where a.Owner == ownerId select a).ToList();

            return articles;
        }

        public static bool validateStr(string str)
        {
            if(string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;

            else
                return true;
        }
    }
}
