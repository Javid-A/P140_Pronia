using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P140_Pronia.Entities
{
    public class Slider
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Offer { get; set; } = null!;
        [Display(Name ="Short description",Prompt = "Please fill this input")]
        public string ShortDesc { get; set; } = null!;
        //[Url]
        public string URL { get; set; } = null!;
        [ValidateNever]
        public string Image { get; set; } = null!;
        public string? BgImage { get; set; } = null!;
        [NotMapped]
        [ValidateNever]
        public IFormFile Photo { get; set; }
        [NotMapped]
        [Display(Name ="Background photo")]
        [ValidateNever]
        public IFormFile BgPhoto { get; set; }
    }
}
