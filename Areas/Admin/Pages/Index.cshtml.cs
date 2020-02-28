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

        public void OnGet(string view)
        {
            var articles = GetArticles(_userManager.GetUserId(User));

            ViewData["articles"] = articles;
        }

        public IActionResult OnGetDeleteArticle(string delete)
        {
            var deleteArticle = _db.article.Find(Guid.Parse(delete));
            deleteArticle.Status = "deleted";
            deleteArticle.Deleted_at = DateTime.Now;
            _db.SaveChanges();

            return Redirect("/admin");
        }

        private List<Articles> GetArticles(string ownerId)
        {
            var user = User.Identity.Name;
            var articles = (from a in _db.article where a.Owner == ownerId select a).ToList();

            return articles;
        }

        public IActionResult OnGetPublished()
        {
            var articles = GetArticles(_userManager.GetUserId(User));
            ViewData["articles"] = (from a in articles where a.Status == "publish" select a).ToList();

            return Page();
        }

        public IActionResult OnGetDraft()
        {
            var articles = GetArticles(_userManager.GetUserId(User));
            ViewData["articles"] = (from a in articles where a.Status == "draft" select a).ToList();

            return Page();
        }

        public IActionResult OnGetDeleted()
        {
            var articles = GetArticles(_userManager.GetUserId(User));
            ViewData["articles"] = (from a in articles where a.Status == "deleted" select a).ToList();

            return Page();
        }

        public IActionResult OnGetRestore(string id)
        {
            var article = _db.article.Find(Guid.Parse(id));
            article.Status = "draft";

            _db.SaveChanges();

            return Redirect("/admin/index?handler=deleted");
        }

        public IActionResult OnGetPublish(string id)
        {
            var article = _db.article.Find(Guid.Parse(id));
            article.Status = "publish";

            _db.SaveChanges();

            return Redirect("/admin/index?handler=draft");
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
