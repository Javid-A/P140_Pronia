using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace P140_Pronia.ViewModels
{
    public class SliderUpdateVM
    {
        public int Id { get; set; }
        [ValidateNever]
        public string Image { get; set; }
        [ValidateNever]
        public string BgImage { get; set; }
        public string Title { get; set; }
        public string ShortDesc { get; set; }
        public string Offer { get; set; }
        public string URL { get; set; }
        [ValidateNever]
        public IFormFile MainPhoto { get; set; }
        [ValidateNever]
        public IFormFile BgPhoto { get; set; }
    }
}
