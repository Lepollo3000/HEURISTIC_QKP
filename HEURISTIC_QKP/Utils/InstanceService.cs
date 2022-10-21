using HEURISTIC_QKP.Models;
using System.Data;
using System.Globalization;
using System.Security.Principal;

namespace HEURISTIC_QKP.Utils
{
    public class InstanceService : IInstanceService
    {
        public Instance? GetInstanceData(string fileName)
        {
            Instance? model = TryGetFileData(fileName);

            return model;
        }

        public InstanceCalculations? GetInstanceCalculations(Instance instance)
        {
            InstanceCalculations model = new InstanceCalculations(instance);

            return model;
        }

        public InstanceSolution? GetInstanceSolution(InstanceCalculations calculations, Instance instance)
        {
            InstanceSolution model = new InstanceSolution(calculations, instance);

            return model;
        }

        private static Instance? TryGetFileData(string fileName)
        {
            // the reference of the instance(r_10_100_13 in the following example)
            // the number of variables(n) (10 in the following example)
            // the linear coefficients(c_i) of the objective function(91 78 22 4 48 85 46 81 3 26)
            // the quadratic coefficients(c_ij) of the objective function
            // a blank line
            // 0 if the constraint is of type <= (i.e.always since we are considering(QKP) instances) and 1 if the constraint is an eglity constraint
            // the capacity of the knapsack(145)
            // the coefficients of the capacity constraints(weights, a_i) (34 33 12 3 43 26 10 2 48 39 )
            // some comments

            try
            {
                string folderPath = AppContext.BaseDirectory + "/FileInstances";
                string filePath = $"{folderPath}/{fileName}.txt";

                if (!string.IsNullOrEmpty(folderPath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        // FILENAME
                        string strInstanceName = sr.ReadLine()!;
                        // ARRAY SIZE
                        string strNumberCoeficients = sr.ReadLine()!;
                        int numberCoeficients = int.Parse(strNumberCoeficients);
                        // LINEAR COEFICIENTS VALUES
                        string strLinearCoeficientsValues = sr.ReadLine()!;
                        int[] linearCoeficientsValues = strLinearCoeficientsValues.Split(" ")
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .Select(l => int.Parse(l)).Reverse().ToArray();

                        Instance instance = new Instance(strInstanceName, numberCoeficients);

                        // DID THIS TO SAVE RESOURCES INSTEAD OF HAVING ALL IN RAM
                        int i = 0;
                        string line;
                        while ((line = sr.ReadLine()!) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line)) break;

                            int[] quadraticCoeficients = line.Split(" ")
                                .Where(l => !string.IsNullOrWhiteSpace(l))
                                .Select(l => int.Parse(l)).Reverse().ToArray();

                            instance.AddQuadraticData(i, quadraticCoeficients);

                            i++;
                        }

                        // 0 USELESS NUMBER VALIDATION
                        string strZeroValue = sr.ReadLine()!;
                        int zeroValue = int.Parse(strZeroValue);
                        // IF THERE'S NO 0, THE FORMAT IS WRONG, SO THROW FORMAT EXCEPTION
                        if (zeroValue != 0) throw new FormatException();

                        // KNAPSACK CAPACITY
                        string strKnapsackCapacity = sr.ReadLine()!;
                        int knapsackCapacity = int.Parse(strKnapsackCapacity);

                        instance.KnapsackCapacity = knapsackCapacity;

                        // LINEAR COEFICIENTS WEIGHTS
                        string strLinearCoeficientsWeights = sr.ReadLine()!;
                        int[] linearCoeficientsWeights = strLinearCoeficientsWeights.Split(" ")
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .Select(l => int.Parse(l)).Reverse().ToArray();

                        instance.AddLinearData(linearCoeficientsWeights, linearCoeficientsValues);

                        return instance;
                    }
                }
            }
            catch (IOException)
            {
                Console.Write(
                    "===============================================================\n" +
                    " Sorry, the file was not found.\n" +
                    " Please verify if the file name is correct or try another one.\n" +
                    "===============================================================\n"
                );
            }
            catch (FormatException)
            {
                Console.Write(
                    "======================================================\n" +
                    " There was a problem with the file format.\n" +
                    " Please try again later or try another instance file.\n" +
                    "======================================================\n"
                );
            }

            return null;
        }
    }
}
