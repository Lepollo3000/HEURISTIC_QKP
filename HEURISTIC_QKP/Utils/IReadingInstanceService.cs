using HEURISTIC_QKP.Models;

namespace HEURISTIC_QKP.Utils
{
    public interface IReadingInstanceService
    {
        Instance GetInstanceData(string directory);
        InstanceCalculations GetInstanceCalculations(Instance instance);
        InstanceSolution GetInstanceSolution(InstanceCalculations calculations, Instance instance);
    }
}
