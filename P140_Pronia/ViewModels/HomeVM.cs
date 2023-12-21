using P140_Pronia.Entities;

namespace P140_Pronia.ViewModels
{
    public class HomeVM
    {
        public int Test;
        public IEnumerable<Slider> Sliders { get; set; } = null!;
        public ICollection<Plant> Plants { get; set; } = null!;
    }
}
