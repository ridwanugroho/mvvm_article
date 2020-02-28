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
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Article.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private ApplicationDbContext _db;

        private readonly ILogger<CreateModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        
        [BindProperty]
        public BufferedSingleFileUploadDb FileUpload { get; set; }

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

        public async Task<IActionResult> OnPost(string title, string content, string edit, string status, [FromForm(Name="files")] IFormFile files)
        {
            Console.WriteLine(title);
            Console.WriteLine(content);
            Console.WriteLine(edit);
            Console.WriteLine(status);

            var path = "wwwroot\\Images";
            var filename = Path.Combine(path, Path.GetRandomFileName());
            Directory.CreateDirectory(path);
            if(files != null)
            {
                using(var stream = new FileStream(filename, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                }
            }


            if(IndexModel.validateStr(edit))
            {
                saveEditArticle(edit, title, content, status);
                Console.WriteLine("saving article .. .");
            }

            else
            {
                createArticle(title, content, status, filename);
                Console.WriteLine("saving new article .. .");
            }


            return RedirectToPage("/SubmitConfirmation");
        }

        private void createArticle(string title, string content, string status, string filename)
        {
            var _status = "publish";
            if(validateStr(status))
                _status = "draft";

            var article  = new Articles()
            {
                Owner = _userManager.GetUserId(User),
                Titile = title,
                Content = content,
                Rating = 10,
                Status = _status,
                Created_at = DateTime.Now,
                Image = filename
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

        public static bool validateStr(string str)
        {
            if(string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
                return false;

            else
                return true;
        }
    }


    public class BufferedSingleFileUploadDb
    {
        // [Required]
        // [Display(Name="File")]
        public IFormFile FormFile { get; set; }
    }
}