using System;
using System.ComponentModel.DataAnnotations;

namespace Article.Model
{
    public class Comments
    {
        [Key]
        public Guid id{get; set;}
    }
}