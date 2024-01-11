using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using P140_Pronia.Entities;
using System.ComponentModel.DataAnnotations;

namespace P140_Pronia.ViewModels
{
    public class PlantUpdateVM
    {
        [ValidateNever]
        public int Id { get; set; }
        [Required]
        [Range(0, 3000)]
        public decimal Price { get; set; }
        [Required]
        [StringLength(maximumLength: 25, MinimumLength = 3)]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [ValidateNever]
        [Display(Name = "Main photo")]
        public IFormFile MainPhoto { get; set; } = null!;
        [ValidateNever]
        [Display(Name = "Hover photo")]
        public IFormFile HoverPhoto { get; set; } = null!;
        [ValidateNever]
        public ICollection<IFormFile> Photos { get; set; } = null!;
        [ValidateNever]
        public ICollection<Category> Categories { get; set; } = null!;
        [ValidateNever]
        public ICollection<Information> Informations { get; set; } = null!;
        [Required]
        [Display(Name="Informations")]
        public List<int> InformationIds { get; set; } = null!;
        [Required]
        [Display(Name="Categories")]
        public List<int> CategoryIds { get; set; } = null!;
        [Required]
        public List<int> PlantImagesIds { get; set; } = null!;
        [ValidateNever]
        public ICollection<PlantImage> PlantImages { get; set; } = null!;
        [ValidateNever]
        public List<PlantCategory> PlantCategories { get; set; } = null!;
        [ValidateNever]
        public List<PlantInformation> PlantInformations { get; set; } = null!;
    }
}
