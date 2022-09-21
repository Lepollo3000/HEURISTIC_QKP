using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEURISTIC_QKP.Models
{
    public class Instance
    {
        public int NumberVariables { get; set; }
        public int KnapsackCapacity { get; set; }
        public List<int> LinearCoeficientsValues { get; set; } = null!;
        public List<int> LinearCoeficientsWeights { get; set; } = null!;
        public List<List<int>> QuadraticCoeficientsValues { get; set; } = null!;
    }
}
