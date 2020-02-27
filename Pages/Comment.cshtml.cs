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
    [Authorize(Roles="user")]
    public class CommentModel : PageModel
    {
        private ApplicationDbContext _db;

        private readonly ILogger<CommentModel> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        private RoleManager<IdentityRole> _roleManager;

        public CommentModel(ILogger<CommentModel> logger, ApplicationDbContext db,
                        UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, 
                        RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }


        public void OnGet()
        {
        }

        public IActionResult OnGetComment(string id)
        {
            Console.WriteLine("get all comment from {0}", id);
            var comments = (from c in _db.comment where c.ArticleId == id select c).ToList();
            
            return new OkObjectResult(comments);
        }

        public IActionResult OnGetSubmitComment(string id, string comment)
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