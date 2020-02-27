using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Article.Model;

namespace Article.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<Articles> article{get; set;}
        public DbSet<Comments> comment{get; set;}
        public DbSet<UserRoles> role{get; set;}
        
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
