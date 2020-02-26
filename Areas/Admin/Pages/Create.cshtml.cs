using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

using Article.Data;
using Article.Model;

namespace Article.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private ApplicationDbContext _db;

        private readonly ILogger<CreateModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public CreateModel(ILogger<CreateModel> logger, ApplicationDbContext db,
                        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public void OnGet(string edit)
        {
            if(!string.IsNullOrEmpty(edit) || !string.IsNullOrWhiteSpace(edit))
            {
                ViewData["articleId"] = edit;
                ViewData["article"] = _db.article.Find(Guid.Parse(edit));
            }

            else
                ViewData["articleId"] = "0";
        }

        public IActionResult OnPost(string title, string content, string edit, string status)
        {
            Console.WriteLine(title);
            Console.WriteLine(content);
            Console.WriteLine(edit);
            Console.WriteLine(status);

            if(IndexModel.validateStr(edit))
            {
                saveEditArticle(edit, title, content, status);
                Console.WriteLine("saving article .. .");
            }

            else
            {
                createArticle(title, content);
                Console.WriteLine("saving new article .. .");
            }

            return RedirectToPage("/SubmitConfirmation");
        }

        private void createArticle(string title, string content)
        {
            var article  = new Articles()
            {
                Owner = _userManager.GetUserId(User),
                Titile = title,
                Content = content,
                Rating = 10,
                Status = "publish",
                Created_at = DateTime.Now,
            };

            _db.article.Add(article);
            _db.SaveChanges();
        }

        private void saveEditArticle(string articleId, string title, string content, string status)
        {
            var article = _db.article.Find(Guid.Parse(articleId));
            article.Titile = title;
            article.Content = content;
            article.Edited_at = DateTime.Now;
            article.Status = status;

            _db.SaveChanges();
        }
    }
}