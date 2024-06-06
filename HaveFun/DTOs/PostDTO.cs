using HaveFun.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HaveFun.DTOs
{
    public class PostDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [MaxLength(2000, ErrorMessage = "文字上限為2000字")]
        public string Contents { get; set; }
        public IFormFile? Pictures { get; set; }
    }
}