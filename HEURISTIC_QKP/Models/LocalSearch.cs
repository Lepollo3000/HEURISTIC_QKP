using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HEURISTIC_QKP.Models
{
    public class LocalSearch
    {
        public Instance Instance { get; set; } = null!;
        public InstanceSolution Solution { get; set; } = null!;
        public InstanceSolution NewSolution { get; set; } = null!;
        public InstanceSolution BestSolution { get; set; } = null!;

        public LocalSearch(InstanceSolution solution, InstanceCalculations calculations, Instance instance)
        {
            Solution = solution;
            Instance = instance;

            // GET THE FIRST RANDOM OBJECT TO BAN FROM RESULT
            List<LinearCoeficient> randomBannedCoeficients = GetRandomBannedLinearCoeficients(Solution.SelectedData, new List<LinearCoeficient>());

            int i = 0;
            do
            {
                // OBTAIN NEW SOLUTION WITH THE EXCLUDED RANDOM OBJECTS
                NewSolution = new InstanceSolution(randomBannedCoeficients, calculations, instance);

                // GET ANOTHER RANDOM OBJECTS TO BAN FROM RESULT
                randomBannedCoeficients = GetRandomBannedLinearCoeficients(Solution.SelectedData, randomBannedCoeficients);
                
                // VALIDATE IF NEW SOLUTION IS BETTER THAN THE ORIGINAL ONE
                if(NewSolution.TotalProfit > Solution.TotalProfit)
                {
                    BestSolution = NewSolution;
                }
                // OR VALIDATE IF THERE'S ANY BEST SOLUTION
                else if (BestSolution == null)
                {
                    BestSolution = Solution;
                }

                // COLLISIONER
                if (i > 1000)
                    break;

                i++;
            }
            while (NewSolution.TotalProfit <= Solution.TotalProfit);
        }

        public void PrintNewSolution()
        {
            Console.Write(
                "\n==========================================================\n" +
                "\t\t\tLocal Search\n" +
                "==========================================================\n\n" +
                $" Total Weight \t\t\t= {BestSolution.TotalWeight}\n" +
                $" Total Profit \t\t\t= {BestSolution.TotalProfit}\n" +
                $" Total Selected Objects \t= {BestSolution.SelectedData.Count}\n\n" +
                 " Selected Objects \t= {\t"
            );

            int i = 1;
            foreach (var data in BestSolution.SelectedData)
            {
                if (i == BestSolution.SelectedData.Count)
                    Console.Write($"{data.ItemNumber}\t");
                else
                    Console.Write($"{data.ItemNumber},\t");

                i++;
            }

            Console.Write(" }\n");
        }

        private List<LinearCoeficient> GetRandomBannedLinearCoeficients(IEnumerable<LinearCoeficient> coeficients, IEnumerable<LinearCoeficient> previousCoeficients)
        {
            var returnModel = new List<LinearCoeficient>();
            var random = new Random();

            foreach (var coeficient in coeficients)
            {
                bool includeObject = random.Next(2) == 1;

                if(includeObject)
                {
                    returnModel.Add(coeficient);
                }
            }

            return returnModel;
        }
    }
}
