
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

        public int MinWeight()
        {
            // GET THE MINIMUM WEIGHT FROM THE MAIN ARRAY OF LINEAR COEFICIENTS
            return LinearCoeficients.Select(lc => lc.Weight).Min();
        }

        public void AddLinearData(int[] w, int[] p)
        {
            // IF THE ARRAY IS NOT A SQUARE ARRANGEMENT, THROW AN EXCEPTION
            if (w.Length != p.Length || w.Length != NumberCoeficients)
                throw new IndexOutOfRangeException($"w and p most be '{NumberCoeficients}' in lenght");

            // SAVE THE LINEAR COEFICIENTS
            for (int i = 0; i < NumberCoeficients; i++)
            {
                // MERGE BOTH GIVEN ARRAYS IN AN OBJECT OF LINEAR COEFICIENT
                LinearCoeficient lc = new LinearCoeficient()
                {
                    ItemNumber = i,
                    Weight = w[i],
                    Profit = p[i]
                };

                // ADD THE CREATED LINEAR COEFICIENT TO THE MAIN ARRAY
                LinearCoeficients[i] = lc;
            }
        }

        public void AddQuadraticData(int i, int[] line)
        {
            // SAVE THE QUADRATIC COEFICIENTS
            for (int j = 0; j < line.Length; j++)
            {
                // MERGE THE GIVEN ARRAY IN AN OBJECT OF QUADRATIC COEFICIENT
                QuadraticCoeficient qc = new QuadraticCoeficient()
                {
                    LinearObjectI = i,
                    LinearObjectJ = j + (i + 1),
                    ExtraProfit = line[j]
                };

                // ADD THE CREATED QUADRATIC COEFICIENT TO THE MAIN ARRAY
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
                    Console.Write($"{QuadraticCoeficients[i, j].ExtraProfit}\t");
                }
                Console.WriteLine("");
            }
        }

        public void PrintInstanceData()
        {
            Console.Write(
                $" Instance Name \t\t\t= {Name}\n" +
                $" Total Objects \t\t\t= {NumberCoeficients}\n" +
                $" Min Weight Object \t\t= {MinWeight()}\n" +
                $" Knapsack Capacity \t\t= {KnapsackCapacity}\n"
            );
        }
    }
}
