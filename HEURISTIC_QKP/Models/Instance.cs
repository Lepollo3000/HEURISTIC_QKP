
namespace HEURISTIC_QKP.Models
{
    public class Instance
    {
        public string Name { get; } 
        public int NumberCoeficients { get; }
        public int KnapsackCapacity { get; set; }
        public LinearCoeficient[] LinearCoeficients { get; }
        public QuadraticCoeficient[,] QuadraticCoeficients { get; }

        public Instance(string name, int numberCoeficients)
        {
            Name = name;
            NumberCoeficients = numberCoeficients;
            LinearCoeficients = new LinearCoeficient[numberCoeficients];
            QuadraticCoeficients = new QuadraticCoeficient[numberCoeficients, numberCoeficients];
        }

        public void AddLinearData(int[] w, int[] p)
        {
            if (w.Length != p.Length || w.Length != NumberCoeficients) 
                throw new IndexOutOfRangeException($"w and p most be '{NumberCoeficients}' in lenght");

            for (int i = 0; i < NumberCoeficients; i++)
            {
                LinearCoeficient lc = new LinearCoeficient()
                {
                    ItemNumber = i,
                    Weight = w[i],
                    Value = p[i]
                };

                LinearCoeficients[i] = lc;
            }
        }

        public void AddQuadraticData(int i, int[] line)
        {
            for (int j = 0; j < line.Length; j++)
            {
                QuadraticCoeficient qc = new QuadraticCoeficient()
                {
                    LinearObjectI = i,
                    LinearObjectJ = j + (i + 1),
                    Value = line[j]
                };

                QuadraticCoeficients[qc.LinearObjectI, qc.LinearObjectJ] = qc;
                QuadraticCoeficients[qc.LinearObjectJ, qc.LinearObjectI] = qc;
            }
        }

        public void ShowQuadraticData()
        {
            for (int i = 0; i < QuadraticCoeficients.GetLength(0); i++)
            {
                for (int j = 0; j < QuadraticCoeficients.GetLength(1); j++)
                {
                    Console.Write($"{QuadraticCoeficients[i, j].Value}\t");
                }
                Console.WriteLine("");
            }
        }

        public void PrintInstanceData()
        {
            Console.Write(
                $" Instance Name \t\t\t= {Name}\n" +
                $" Total Objects \t\t\t= {NumberCoeficients}\n" +
                $" Knapsack Capacity \t\t= {KnapsackCapacity}\n"
            );
        }
    }
}
