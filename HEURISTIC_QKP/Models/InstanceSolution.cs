
namespace HEURISTIC_QKP.Models
{
    public class InstanceSolution
    {
        public List<InstanceSelectedData> SelectedData { get; set; } = null!;
        public int TotalWeight { get; set; }
        public int TotalValue { get; set; }
    }
}
