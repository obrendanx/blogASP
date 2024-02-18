using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TestProject.Models
{
    public class BlogPost
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string PostedBy { get; set; }
        [Required]
        public DateTime CreatedDateTime { get; set; } = DateTime.Now;
        [Required]
        public string ForumPage { get; set; }
    }
}
