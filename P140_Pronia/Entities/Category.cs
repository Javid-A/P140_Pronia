using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace P140_Pronia.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
        [ValidateNever]
        public ICollection<PlantCategory> PlantCategories { get; set; }
    }
}
