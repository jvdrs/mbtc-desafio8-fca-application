using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FcaApplication.Api.Models
{
    public class FileModel
    {
        [Required]
        public string Car { get; set; }

        [Required]
        public IFormFile Audio { get; set; }
    }
}