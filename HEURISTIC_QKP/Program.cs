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

                Instance instance = service.GetInstanceData(directory)!;

                if (instance != null)
                {
                    Console.WriteLine("");

                    instance.PrintInstanceData();

                    Console.WriteLine("");

                    InstanceCalculations calculations = service.GetInstanceCalculations(instance)!;

                    if (calculations != null)
                    {
                        InstanceSolution solution = service.GetInstanceSolution(calculations, instance)!;

                        if (solution != null)
                        {
                            solution.PrintSolution();
                            watch.Stop();
                            Console.WriteLine($"\nElapsed Time: {watch.ElapsedMilliseconds} ms.\n");

                            watch.Reset();
                            watch.Start();
                            LocalSearch localSearch = new LocalSearch(solution, calculations, instance)!;

                            if (localSearch != null)
                            {
                                localSearch.PrintNewSolution();
                                watch.Stop();
                                Console.WriteLine($"\nElapsed Time: {watch.ElapsedMilliseconds} ms.\n");
                            }
                        }
                    }

                    Console.WriteLine("");
                }

                watch.Stop();

                Console.WriteLine($"Elapsed Time: {watch.ElapsedMilliseconds} ms.\n");
            }

            Console.WriteLine("Press any key to exit...");

            Console.ReadKey();
        }
    }
}