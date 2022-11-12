using HEURISTIC_QKP.Models;
using HEURISTIC_QKP.Utils;
using System.Diagnostics;

namespace HEURISTIC_QKP
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceService service = new InstanceService();

            Console.Write(
                "==========================================================\n" +
                "\t\tQuadratic Knapsack Problem\n" +
                "----------------------------------------------------------\n" +
                " If you know the name of the instance you want to test\n" +
                " Type it, please: ~ "
            );

            string? directory = Console.ReadLine();

            Console.Write(
                "----------------------------------------------------------\n" +
                " Reading file, please wait...\n" +
                "==========================================================\n"
            );

            if (!string.IsNullOrEmpty(directory))
            {
                Stopwatch watch = Stopwatch.StartNew();

                Console.Clear();

                // INSTANCE INFORMATION
                Instance instance = service.GetInstanceData(directory)!;
                // INSTANCE BEST SOLUTION
                InstanceSolution bestSolution = null!;

                if (instance != null)
                {
                    Console.WriteLine("");

                    instance!.PrintInstanceData();

                    Console.WriteLine("");

                    for (int i = 0; i < 10; i++)
                    {
                        InstanceCalculations calculations = service.GetInstanceCalculations(instance)!;

                        if (calculations != null)
                        {
                            InstanceSolution solution = service.GetInstanceSolution(calculations, instance)!;

                            if (solution != null)
                            {
                                LocalSearch localSearch = new LocalSearch(solution, calculations, instance)!;

                                if (localSearch != null)
                                {
                                    if (bestSolution != null && localSearch.BestSolution.TotalProfit > bestSolution.TotalProfit)
                                    {
                                        bestSolution = localSearch.BestSolution;
                                    }
                                    else if (bestSolution == null)
                                    {
                                        bestSolution = localSearch.BestSolution;
                                    }
                                }
                            }
                        }
                    }
                }

                bestSolution!.PrintSolution();

                watch.Stop();
                Console.WriteLine($"\nElapsed Time: {watch.ElapsedMilliseconds} ms.\n");

                Console.WriteLine("");
            }

            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
        }
    }
}