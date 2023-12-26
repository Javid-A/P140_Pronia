using P140_Pronia.Entities;

namespace P140_Pronia.ViewModels
{
    public class PlantDetailsVM
    {
        public Plant Plant { get; set; } = null!;
        public ICollection<Plant> Relateds { get; set; } = null!;
    }
}
