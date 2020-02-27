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
    public class ArticleDetailModel : PageModel
    {
        private ApplicationDbContext _db;

        private readonly ILogger<ArticleDetailModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ArticleDetailModel(ILogger<ArticleDetailModel> logger, ApplicationDbContext db,
                        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public void OnGet(string id)
        {
            var article = _db.article.Find(Guid.Parse(id));

            ViewData["article"] = article;
        }

//{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{{Comment handler}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}}

        public IActionResult OnGetComment(string id)
        {
            Console.WriteLine("get all comment from {0}", id);
            var comments = (from c in _db.comment where c.ArticleId == id select c).ToList();
            
            return new OkObjectResult(comments);
        }

        [Authorize(Roles="user")]
        public IActionResult OnGetSubmitComment(string id, string comment)
        {
            if(validateUser(_userManager.GetUserId(User), "user"))
            {
                var commentToSend = new Comments()
                {
                    Sender = _userManager.GetUserId(User),
                    ArticleId = id,
                    Content = comment,
                    Created_at = DateTime.Now
                };

                _db.comment.Add(commentToSend);
                _db.SaveChanges();

                return new OkObjectResult(commentToSend);
            }

            else
                return new OkObjectResult(new Comments(){});
        }

        private bool validateUser(string id, string role)
        {
            var emptyUser = "00000000-0000-0000-0000-000000000000";
            
            if(id == emptyUser)
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