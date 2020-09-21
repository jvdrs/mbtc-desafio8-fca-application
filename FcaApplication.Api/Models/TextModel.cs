using System.ComponentModel.DataAnnotations;

namespace FcaApplication.Api.Models
{
    public class TextModel
    {
        [Required]
        public string Text { get; set; }

        [Required]
        public string Car { get; set; }
    }
}