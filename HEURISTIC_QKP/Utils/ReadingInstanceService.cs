using HEURISTIC_QKP.Models;
using System.Data;

namespace HEURISTIC_QKP.Utils
{
    public class ReadingInstanceService : IReadingInstanceService
    {
        public Instance GetInstanceData(string fileName)
        {
            var model = TryGetFile(fileName);

            return model;
        }

        public InstanceCalculations GetInstanceCalculations(Instance instance)
        {
            List<float> listRelationWeightProfit = instance.LinearCoeficients.Select(cl => (float)cl.Weight / (float)cl.Value).ToList();
            List<int> listRelationProductProduct = GetInstanceRPP(instance);
            List<float> listRelationOfRelations = new List<float>();
            List<InstanceRelation> calculations = new List<InstanceRelation>();

            for (int i = 0; i < instance.LinearCoeficients.Count(); i++)
            {
                float division = listRelationProductProduct[i] / listRelationWeightProfit[i];

                listRelationOfRelations.Add(division);
                InstanceRelation calculation = new InstanceRelation()
                {
                    Weight = instance.LinearCoeficients[i].Weight,
                    Value = instance.LinearCoeficients[i].Value,
                    RelationWeightProfit = listRelationWeightProfit[i],
                    RelationProductProduct = listRelationProductProduct[i],
                    RelationOfRelations = listRelationOfRelations[i]
                };

                calculations.Add(calculation);
            }

            calculations = calculations.OrderBy(c => c.RelationOfRelations).ToList();

            InstanceCalculations model = new InstanceCalculations()
            {
                Relations = calculations
            };

            return model;
        }

        public InstanceSolution GetInstanceSolution(InstanceCalculations calculations, Instance instance)
        {
            int totalWeight = 0, totalValue = 0;
            decimal combinedValues = 0;
            List<LinearCoeficient> selectedData = new List<LinearCoeficient>();

            foreach (var item in calculations.Relations)
            {
                Console.WriteLine(item.Weight);
                if (totalWeight + item.Weight < instance.KnapsackCapacity)
                {
                    totalWeight += item.Weight;
                    totalValue += item.Value;

                    selectedData.Add(instance.LinearCoeficients
                        .Where(lc => lc.Weight == item.Weight && lc.Value == item.Value).First());
                }
            }

            foreach(var item in selectedData)
            {
                var selectedQuadratic = instance.QuadraticCoeficients[item.ItemNumber];
            }

            InstanceSolution model = new InstanceSolution()
            {
                SelectedData = selectedData.Select(sd => new InstanceSelectedData() { SelectedData = selectedData}).ToList(),
                TotalWeight = totalWeight,
                TotalValue = totalValue
            };

            return model;
        }

        private Instance? TryGetFile(string fileName)
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
                var directory = new DirectoryInfo(folderPath);

                if (directory.Exists)
                {
                    FileInfo? file = directory.GetFiles($"{fileName}*", SearchOption.TopDirectoryOnly).FirstOrDefault();

                    if (file != null)
                    {
                        Instance instance = new Instance();

                        // GET ALL LINES OF FILE
                        string[] fileLines = File.ReadAllLines(file.FullName);

                        // NUMBER OF COEFICIENTS
                        int numberCoeficients = int.Parse(fileLines[1]);

                        // DO A LIST OF THE THIRD LINE TO GET LINEAR COEFICIENT VALUES
                        string[] linearValuesLine = new string[numberCoeficients];
                        linearValuesLine = fileLines[2].Split(" ")
                            .Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();
                        // DO A LIST OF THE LAST LINE TO GET LINEAR COEFICIENT WEIGHTS
                        string[] linearWeightsLine = fileLines[numberCoeficients + 5].Split(" ")
                            .Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

                        // DO A LIST OF LISTS TO GET QUADRATIC COEFICIENTS
                        string[][] quadraticValuesLines = new string[numberCoeficients][];

                        // FILL THE LIST OF QUADRATIC COEFICIENTS FROM THE FOURTH LINE UNTIL THE LAST ONE
                        for (int i = 3; i < numberCoeficients + 3; i++)
                        {
                            string[] line = new string[numberCoeficients];
                            line = fileLines[i].Split(" ")
                                .Where(l => !string.IsNullOrWhiteSpace(l)).ToArray();

                            quadraticValuesLines[i - 3] = line;
                        }

                        var linearValues = linearValuesLine.Select(l => int.Parse(l)).ToList();
                        var linearWeights = linearWeightsLine.Select(l => int.Parse(l)).ToList();
                        var quadraticValues = quadraticValuesLines.Select(l => l.Select(q => int.Parse(q)).ToList()).ToList();
                        var linearCoeficients = new List<LinearCoeficient>();

                        for (int i = 0; i < numberCoeficients; i++)
                        {
                            var coeficient = new LinearCoeficient()
                            {
                                ItemNumber = i,
                                Value = linearValues[i],
                                Weight = linearWeights[i]
                            };

                            linearCoeficients.Add(coeficient);
                            //linearC.Select(c => new LinearCoeficient() { Value = c.Value, Weight = c.Weight }).ToList();
                        }

                        var quadraticCoeficients = quadraticValues
                            .Select(lq => lq.Select(q => new QuadraticCoeficient() { Value = q }).ToList()).ToList();

                        int i = 0;
                        foreach(var list in quadraticCoeficients)
                        {
                            int j = 0;
                            foreach(var item in list)
                            {
                                quadraticCoeficients[i][j].LinearObjectI = i;
                                quadraticCoeficients[i][j].LinearObjectJ = j;
                            }

                            i++;
                        }

                        // FILL THE FIRST INSTANCE DATA :D
                        instance.Name = fileLines[0];
                        instance.NumberCoeficients = int.Parse(fileLines[1]);
                        instance.KnapsackCapacity = int.Parse(fileLines[numberCoeficients + 4]);
                        instance.LinearCoeficients = linearCoeficients;
                        instance.QuadraticCoeficients = quadraticCoeficients;

                        return instance;
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.Write(
                    "----------------------------------------------------------\n" +
                    "\t\tSorry, the file was not found.\n" +
                    "\t\tPlease try with another one.\n" +
                    "----------------------------------------------------------\n"
                );
            }
            catch (FormatException)
            {
                Console.Write(
                    "----------------------------------------------------------\n" +
                    "\t\tThere was a problem with the format of the file.\n" +
                    "\t\tPlease try again later or change the instance file.\n" +
                    "----------------------------------------------------------\n"
                );
            }

            return null;
        }

        private List<int> GetInstanceRPP(Instance instance)
        {
            var listRelationProductProduct = new List<int>();
            int escalon = 1;

            for (int i = 0; i < instance.QuadraticCoeficients.Count(); i++)
            {
                int sum = 0;

                for (int j = 0; j <= instance.QuadraticCoeficients.Count() - escalon; j++)
                {
                    if (j == instance.QuadraticCoeficients.Count() - escalon - 1)
                    {

                        Console.Write($"{instance.QuadraticCoeficients[i][j].Value}\t");
                        sum += instance.QuadraticCoeficients[i][j].Value;

                        int escalon2 = 1;

                        if (i != 0)
                        {
                            for (int k = i; Math.Abs(i) >= escalon2; k--)
                            {
                                if (k > 0)
                                    sum += instance.QuadraticCoeficients[i - escalon2][j + 1].Value;

                                escalon2++;
                            }
                        }

                        break;
                    }
                    else if (j == instance.QuadraticCoeficients.Count() - escalon)
                    {
                        for (int k = 0; k < instance.QuadraticCoeficients.Count() - 1; k++)
                            sum += instance.QuadraticCoeficients[k][0].Value;

                        break;
                    }

                    Console.Write($"{instance.QuadraticCoeficients[i][j].Value}\t");
                    sum += instance.QuadraticCoeficients[i][j].Value;
                }

                escalon++;
                Console.WriteLine("");
                listRelationProductProduct.Add(sum);
            }

            return listRelationProductProduct;
        }
    }
}
