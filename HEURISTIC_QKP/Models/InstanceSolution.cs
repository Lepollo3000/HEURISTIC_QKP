
namespace HEURISTIC_QKP.Models
{
    public class InstanceSolution
    {
        public List<LinearCoeficient> SelectedData { get; set; } = null!;
        public int TotalWeight { get; set; }
        public int TotalValue { get; set; }

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
