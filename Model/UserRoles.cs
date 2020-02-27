using System;

namespace Article.Model
{
    public class UserRoles
    {
        public Guid id{get; set;}
        public string UserId{get; set;}
        public int Role{get; set;}
        public DateTime Created_at{get; set;}
        public DateTime Edited_at{get; set;}     
        public DateTime Deleted_at{get; set;}
    }
}