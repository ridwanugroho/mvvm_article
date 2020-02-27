using System;
using System.ComponentModel.DataAnnotations;

namespace Article.Model
{
    public class Comments
    {
        [Key]
        public Guid id{get; set;}
        public string Sender{get; set;}
        public string ArticleId{get; set;}
        public string Content{get; set;}
        public DateTime Created_at{get; set;}
        public DateTime Deleted_at{get; set;}
    }
}