using P140_Pronia.Entities;
using System.Diagnostics.CodeAnalysis;

namespace P140_Pronia.Helpers
{
    public class PlantComparer : IEqualityComparer<Plant>
    {
        public bool Equals(Plant? x, Plant? y)
        {
            if (x?.Id == y?.Id) return true;
            return false;
        }

        public int GetHashCode([DisallowNull] Plant obj)
        {
            throw new NotImplementedException();
        }
    }
}
