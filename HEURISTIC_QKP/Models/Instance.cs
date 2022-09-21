
namespace HEURISTIC_QKP.Models
{
    public class Instance
    {
        public string Name { get; set; } = null!;
        public int NumberCoeficients { get; set; }
        public int KnapsackCapacity { get; set; }
        public List<LinearCoeficient> LinearCoeficients { get; set; } = null!;
        public List<List<QuadraticCoeficient>> QuadraticCoeficients { get; set; } = null!;
    }
}
