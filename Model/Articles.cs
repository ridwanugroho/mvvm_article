using System;

namespace Article.Model
{
    public class Articles
    {
        public Guid id{get; set;}
        public string Owner{get; set;}
        public string Titile{get; set;}
        public string Content{get; set;}
        public ushort Rating{get; set;}
        public string Status{get; set;}
        public DateTime Created_at{get; set;}
        public DateTime Edited_at{get; set;}
        public DateTime Deleted_at{get; set;}
    }
}