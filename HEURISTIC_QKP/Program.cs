using HEURISTIC_QKP.Models;
using HEURISTIC_QKP.Utils;

namespace HEURISTIC_QKP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ReadingInstanceService service = new ReadingInstanceService();

            Console.Write(
                "==========================================================\n" +
                "\t\tQuadratic Knapsack Problem\n" +
                "==========================================================\n" +
                " If you know the name of the instance you want to test...\n" +
                " Type it, please: ~ "
            );

            string? directory = Console.ReadLine();

            if (!string.IsNullOrEmpty(directory))
            {
                Instance instance = service.GetInstanceData(directory);

                if (instance != null)
                {
                    InstanceCalculations calculations = service.GetInstanceCalculations(instance);

                    if(calculations != null)
                    {
                        InstanceSolution solution = service.GetInstanceSolution(calculations, instance);
                    }
                }
            }

        }
    }
}