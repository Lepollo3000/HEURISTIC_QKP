
using System;
using System.Collections.Generic;

namespace HEURISTIC_QKP.Models
{
    public class InstanceSolution
    {
        public List<LinearCoeficient> SelectedData { get; set; } = null!;
        public int TotalWeight { get; set; }
        public int TotalProfit { get; set; }

        private int KIndex = 4;

        public InstanceSolution(IEnumerable<LinearCoeficient> bannedCoeficients, InstanceCalculations calculations, Instance instance)
        {
            // UPDATE KINDEX TO THE 30% OF NUMBER OF LINEAR COEFICIENTS
            KIndex = (int)(instance.LinearCoeficients.Count() * .3);

            bool KnapsackHasFreeSpace = true;
            int totalWeight = 0, totalProfit = 0;

            var random = new Random();
            List<LinearCoeficient> selectedData = new List<LinearCoeficient>();

            // SUM OF WEIGHTS AND VALUES WITHOUT EXCEEDING KNAPSACK CAPACITY
            while (KnapsackHasFreeSpace)
            {
                // GET AT MAXIMUM THE FIRST 4 BEST ITEMS IF THE WEIGHT OF THE LINEAR COEFICIENT THAT IS
                // CURRENTLY EVALUATED DONT EXCEED THE KNAPSACK CAPACITY AND IS NOT PART OF BANNED COEFICIENTS
                IEnumerable<LinearCoeficient> bestSelected = calculations.Relations
                    .Where(a => bannedCoeficients.All(b => b.ItemNumber != a.LinearCoeficient.ItemNumber))
                    .Where(a => selectedData.All(b => b.ItemNumber != a.LinearCoeficient.ItemNumber))
                    .Where(a => totalWeight + a.LinearCoeficient.Weight <= instance.KnapsackCapacity)
                    .Select(a => a.LinearCoeficient).Take(KIndex).ToList();

                // IF LINQ GET ANY ITEM, THERE'S STILL SOMETHING TO DO
                if (bestSelected.Any())
                {
                    // SELECT RANDOMLY 1 OF THE POSSIBLE 4 ELEMENTS
                    int randomIndex = random.Next(bestSelected.Count());
                    LinearCoeficient randomlySelected = bestSelected.ElementAt(randomIndex);

                    // ADD THE VALUE AND WEIGHT TO THE TOTAL SUM
                    totalWeight += randomlySelected.Weight;
                    totalProfit += randomlySelected.Profit;

                    // ADD THE LINEAR COEFICIENT TO THE MAIN LIST OF SELECTED DATA
                    selectedData.Add(instance.LinearCoeficients
                        .Where(lc => lc.ItemNumber == randomlySelected.ItemNumber).First());
                }
                // IF NOT, STOP THE WHILE
                else
                {
                    KnapsackHasFreeSpace = false;
                }
            }

            // SUM OF COMBINATORIAL PROFIT OF SELECTED DATA
            for (int i = 0; i < selectedData.Count - 1; i++)
            {
                for (int j = i + 1; j < selectedData.Count; j++)
                {
                    totalProfit += instance.QuadraticCoeficients[selectedData[i].ItemNumber, selectedData[j].ItemNumber].ExtraProfit;
                }
            }

            // ORDER THE FINAL LIST BY ITEM NUMBER
            SelectedData = selectedData.OrderBy(s => s.ItemNumber).ToList();
            // ADD THE TOTAL SUMATORY OF WEIGHT TO THE MAIN PROPERTY
            TotalWeight = totalWeight;
            // ADD THE TOTAL SUMATORY OF PROFIT TO THE MAIN PROPERTY
            TotalProfit = totalProfit;
        }

        public InstanceSolution(InstanceCalculations calculations, Instance instance)
        {
            // UPDATE KINDEX TO THE 30% OF NUMBER OF LINEAR COEFICIENTS
            KIndex = (int)(instance.LinearCoeficients.Count() * .3);

            bool KnapsackHasFreeSpace = true;
            int totalWeight = 0, totalProfit = 0;

            var random = new Random();
            List<LinearCoeficient> selectedData = new List<LinearCoeficient>();

            // SUM OF WEIGHTS AND VALUES WITHOUT EXCEEDING KNAPSACK CAPACITY
            while (KnapsackHasFreeSpace)
            {
                // GET AT MAXIMUM THE FIRST 4 BEST ITEMS IF THE WEIGHT OF THE LINEAR COEFICIENT THAT IS
                // CURRENTLY EVALUATED DONT EXCEED THE KNAPSACK CAPACITY
                IEnumerable<LinearCoeficient> bestSelected = calculations.Relations
                    .Where(a => selectedData.All(b => b.ItemNumber != a.LinearCoeficient.ItemNumber))
                    .Where(a => totalWeight + a.LinearCoeficient.Weight <= instance.KnapsackCapacity)
                    .Select(a => a.LinearCoeficient).Take(KIndex).ToList();

                // IF LINQ GET ANY ITEM, THERE'S STILL SOMETHING TO DO
                if (bestSelected.Any())
                {
                    // SELECT RANDOMLY 1 OF THE POSSIBLE 4 ELEMENTS
                    LinearCoeficient randomlySelected = bestSelected.ElementAt(random.Next(bestSelected.Count()));

                    // ADD THE VALUE AND WEIGHT TO THE TOTAL SUM
                    totalWeight += randomlySelected.Weight;
                    totalProfit += randomlySelected.Profit;

                    // ADD THE LINEAR COEFICIENT TO THE MAIN LIST OF SELECTED DATA
                    selectedData.Add(instance.LinearCoeficients
                        .Where(lc => lc.ItemNumber == randomlySelected.ItemNumber).First());
                }
                // IF NOT, STOP THE WHILE
                else
                {
                    KnapsackHasFreeSpace = false;
                }
            }

            // SUM OF COMBINATORIAL PROFIT OF SELECTED DATA
            for (int i = 0; i < selectedData.Count - 1; i++)
            {
                for (int j = i + 1; j < selectedData.Count; j++)
                {
                    totalProfit += instance.QuadraticCoeficients[selectedData[i].ItemNumber, selectedData[j].ItemNumber].ExtraProfit;
                }
            }

            // ORDER THE FINAL LIST BY ITEM NUMBER
            SelectedData = selectedData.OrderBy(s => s.ItemNumber).ToList();
            // ADD THE TOTAL SUMATORY OF WEIGHT TO THE MAIN PROPERTY
            TotalWeight = totalWeight;
            // ADD THE TOTAL SUMATORY OF PROFIT TO THE MAIN PROPERTY
            TotalProfit = totalProfit;
        }

        public void PrintSolution()
        {
            Console.Write(
                "\n==========================================================\n" +
                "\t\t\tInstance Solution\n" +
                "==========================================================\n\n" +
                $" Total Weight \t\t\t= {TotalWeight}\n" +
                $" Total Profit \t\t\t= {TotalProfit}\n" +
                $" Total Selected Objects \t= {SelectedData.Count}\n\n" +
                 " Selected Objects \t= {\t"
            );

            int i = 1;
            foreach (var data in SelectedData)
            {
                if (i == SelectedData.Count)
                    Console.Write($"{data.ItemNumber}\t");
                else
                    Console.Write($"{data.ItemNumber},\t");

                i++;
            }

            Console.Write(" }\n");
        }
    }
}
