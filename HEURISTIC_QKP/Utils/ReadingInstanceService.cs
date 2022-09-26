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
            var model = TryGetFileData(fileName);

            return model;
        }

        public InstanceCalculations? GetInstanceCalculations(Instance instance)
        {
            float[] listRelationWeightProfit = instance.LinearCoeficients.Select(cl => (float)cl.Value / (float)cl.Weight).ToArray();
            int[] listRelationProductProduct = GetInstanceRPP(instance);

            InstanceRelation[] calculations = new InstanceRelation[instance.NumberCoeficients];

            for (int i = 0; i < instance.LinearCoeficients.Count(); i++)
            {
                float division = listRelationProductProduct[i] / listRelationWeightProfit[i];

                InstanceRelation calculation = new InstanceRelation()
                {
                    LinearCoeficient = instance.LinearCoeficients[i],
                    RelationWeightProfit = listRelationWeightProfit[i],
                    RelationProductProduct = listRelationProductProduct[i],
                    RelationOfRelations = division
                };

                calculations[i] = calculation;
            }

            calculations = calculations.OrderBy(c => c.RelationOfRelations).ToArray();

            InstanceCalculations model = new InstanceCalculations()
            {
                Relations = calculations
            };

            return model;
        }

        public InstanceSolution? GetInstanceSolution(InstanceCalculations calculations, Instance instance)
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

            InstanceSolution model = new InstanceSolution()
            {
                SelectedData = selectedData.OrderBy(s => s.ItemNumber).ToList(),
                TotalWeight = totalWeight,
                TotalValue = totalValue
            };

            return model;
        }

        private Instance? TryGetFileData(string fileName)
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
                        string line;
                        // FILENAME
                        string strInstanceName = sr.ReadLine();
                        // ARRAY SIZE
                        string strNumberCoeficients = sr.ReadLine();
                        int numberCoeficients = int.Parse(strNumberCoeficients);
                        // LINEAR COEFICIENTS VALUES
                        string strLinearCoeficientsValues = sr.ReadLine();
                        int[] linearCoeficientsValues = strLinearCoeficientsValues.Split(" ")
                            .Where(l => !string.IsNullOrWhiteSpace(l))
                            .Select(l => int.Parse(l)).Reverse().ToArray();

                        Instance instance = new Instance(strInstanceName, numberCoeficients);

                        int i = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line) || string.IsNullOrEmpty(line)) break;

                            int[] quadraticCoeficients = line.Split(" ")
                                .Where(l => !string.IsNullOrWhiteSpace(l))
                                .Select(l => int.Parse(l)).Reverse().ToArray();

                            instance.AddQuadraticData(i, quadraticCoeficients);

                            i++;
                        }

                        // 0 USELESS NUMBER VALIDATION
                        string strZeroValue = sr.ReadLine();
                        int zeroValue = int.Parse(strZeroValue);

                        // IF THERE'S NO 0 THE FORMAT IS WRONG
                        if (zeroValue != 0) throw new FormatException();

                        // KNAPSACK CAPACITY
                        string strKnapsackCapacity = sr.ReadLine();
                        int knapsackCapacity = int.Parse(strKnapsackCapacity);

                        instance.KnapsackCapacity = knapsackCapacity;

                        // LINEAR COEFICIENTS WEIGHTS
                        string strLinearCoeficientsWeights = sr.ReadLine();
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
                    " There was a problem with the format of the file.\n" +
                    " Please try again later or try another instance file.\n" +
                    "======================================================\n"
                );
            }

            return null;
        }

        private int[] GetInstanceRPP(Instance instance)
        {
            int[] relationProductProduct = new int[instance.NumberCoeficients];

            for (int i = 0; i < instance.NumberCoeficients; i++)
            {
                int sum = 0;

                for (int j = 0; j < instance.NumberCoeficients; j++)
                {
                    sum += instance.QuadraticCoeficients[i, j].Value;
                }

                relationProductProduct[i] = sum;
            }

            return relationProductProduct;
        }
    }
}
