using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FcaApplication.Api.Models
{
    public class RecommendModel
    {
        [Required]
        public string Car { get; set; }

        public string Text { get; set; }

        public IFormFile Audio { get; set; }
    }
}