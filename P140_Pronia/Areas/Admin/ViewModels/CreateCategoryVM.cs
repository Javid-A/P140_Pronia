using System.ComponentModel.DataAnnotations;

namespace P140_Pronia.Areas.Admin.ViewModels
{
    public class CreateCategoryVM
    {
        [Required(ErrorMessage ="Zehmet olmasa doldurun")]
        //[StringLength(maximumLength: 3)]
        public string Name { get; set; } = null!;
    }
}
