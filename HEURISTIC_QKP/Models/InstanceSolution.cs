
namespace HEURISTIC_QKP.Models
{
    public class InstanceSolution
    {
        public List<LinearCoeficient> SelectedData { get; set; } = null!;
        public int TotalWeight { get; set; }
        public int TotalProfit { get; set; }

        public InstanceSolution(InstanceCalculations calculations, Instance instance)
        {
            int totalWeight = 0, totalProfit = 0;
            List<LinearCoeficient> selectedData = new List<LinearCoeficient>();

            // SUM OF WEIGHTS AND VALUES WITHOUT EXCEEDING KNAPSACK CAPACITY
            foreach (var item in calculations.Relations)
            {
                // IF THE WEIGHT OF THE LINEAR COEFICIENT THAT IS CURRENTLY EVALUATED DONT EXCEED
                // THE KNAPSACK CAPACITY
                if (totalWeight + item.LinearCoeficient.Weight <= instance.KnapsackCapacity)
                {
                    // ADD THE VALUE AND WEIGHT TO THE TOTAL SUM
                    totalWeight += item.LinearCoeficient.Weight;
                    totalProfit += item.LinearCoeficient.Profit;

                    // ADD THE LINEAR COEFICIENT TO THE MAIN LIST OF SELECTED DATA
                    selectedData.Add(instance.LinearCoeficients
                        .Where(lc => lc.ItemNumber == item.LinearCoeficient.ItemNumber).First());
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
