using P140_Pronia.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace P140_Pronia.ViewModels
{
    public class PlantCreateVM
    {
        [Required]
        [Range(0,3000)]
        public decimal Price { get; set; }
        [Required]
        public string SKU { get; set; } = null!;
        [Required]
        [StringLength(maximumLength:25, MinimumLength =3)]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        [Display(Name = "Main photo")]
        public IFormFile MainPhoto { get; set; } = null!;
        [Required]
        [Display(Name = "Hover photo")]
        public IFormFile HoverPhoto { get; set; } = null!;
        [Required]
        public ICollection<IFormFile> Photos { get; set; } = null!;
        public ICollection<Category> Categories { get; set; } = null!;
        public ICollection<Information> Informations { get; set; } = null!;
        [Required]
        public ICollection<int> InformationIds { get; set; } = null!;
        [Required]
        public ICollection<int> CategoryIds { get; set; } = null!;
    }
}
