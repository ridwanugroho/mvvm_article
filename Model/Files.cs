using System;

namespace Article.Model
{
    public class AppFile
    {
        public Guid id{get; set;}
        public string Image{get; set;}
        public string ArticleID{get; set;}
        public DateTime Created_at{get; set;}
        public DateTime Edited_at{get; set;}
    }
}