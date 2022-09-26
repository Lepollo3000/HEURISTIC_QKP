
namespace HEURISTIC_QKP.Models
{
    public class InstanceSolution
    {
        public List<LinearCoeficient> SelectedData { get; set; } = null!;
        public int TotalWeight { get; set; }
        public int TotalValue { get; set; }

        public InstanceSolution(InstanceCalculations calculations, Instance instance)
        {
            int totalWeight = 0, totalValue = 0;
            List<LinearCoeficient> selectedData = new List<LinearCoeficient>();

            // SUM OF WEIGHTS AND VALUES WITHOUT EXCEEDING KNAPSACK CAPACITY
            foreach (var item in calculations.Relations)
            {
                if (totalWeight + item.LinearCoeficient.Weight < instance.KnapsackCapacity)
                {
                    totalWeight += item.LinearCoeficient.Weight;
                    totalValue += item.LinearCoeficient.Value;

                    selectedData.Add(instance.LinearCoeficients
                        .Where(lc => lc.ItemNumber == item.LinearCoeficient.ItemNumber).First());
                }
            }

            // SUM OF COMBINATORIAL VALUES OF SELECTED DATA
            for (int i = 0; i < selectedData.Count - 1; i++)
            {
                for (int j = i + 1; j < selectedData.Count; j++)
                {
                    totalValue += instance.QuadraticCoeficients[selectedData[i].ItemNumber, selectedData[j].ItemNumber].Value;
                }
            }

            SelectedData = selectedData.OrderBy(s => s.ItemNumber).ToList();
            TotalWeight = totalWeight;
            TotalValue = totalValue;
        }

        public void PrintSolution()
        {
            Console.Write(
                $" Total Weight \t\t\t= {TotalWeight}\n" +
                $" Total Value \t\t\t= {TotalValue}\n" +
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
