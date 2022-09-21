using HEURISTIC_QKP.Models;
using System.Data;

namespace HEURISTIC_QKP.Utils
{
    public class ReadingInstanceService : IReadingInstanceService
    {
        public Instance GetInstanceData(string fileName)
        {
            var model = TryGetFile(fileName);

            if (model != null)
                return model;

            return null;
        }

        public InstanceCalculations GetInstanceCalculations(Instance instance)
        {
            InstanceCalculations calculations = new InstanceCalculations();
            int totalCoeficients = instance.NumberCoeficients;

            calculations.RelationWeightProfit = instance.LinearCoeficients.Select(cl => (float)cl.Weight / (float)cl.Value).ToList();

            var listRelationProductProduct = new List<int>();
            int escalon = 1;
            for (int i = 0; i < instance.QuadraticCoeficients.Count(); i++)
            {
                int sum = 0;

                for (int j = 0; j < instance.QuadraticCoeficients.Count() - escalon; j++)
                {
                    if (j == instance.QuadraticCoeficients.Count() - i)
                        break;

                    Console.Write(instance.QuadraticCoeficients[i][j].Value + "\t");
                    sum += instance.QuadraticCoeficients[i][j].Value;
                    //Console.Write($"{i},{j}\t");
                    /*
                    int count = 1;
                    if (j < totalCoeficients - i - 1)
                        sum += instance.QuadraticCoeficients[i][j].Value;
                    else
                    {
                        count++;
                        sum += instance.QuadraticCoeficients[i - count][j + 1].Value;
                    }
                    */
                }
                escalon++;
                Console.WriteLine("");
                listRelationProductProduct.Add(sum);
            }

            return null;
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
                        var linearC = linearValues.Join(linearWeights,
                            lv => lv, lw => lw,
                            (lv, lw) => new { Value = lv, Weight = lw }
                        ).ToList();
                        var linearCoeficients = new List<LinearCoeficient>();

                        for (int i = 0; i < numberCoeficients; i++)
                        {
                            var coeficient = new LinearCoeficient()
                            {
                                Value = linearValues[i],
                                Weight = linearWeights[i]
                            };

                            linearCoeficients.Add(coeficient);
                            //linearC.Select(c => new LinearCoeficient() { Value = c.Value, Weight = c.Weight }).ToList();
                        }

                        var quadraticCoeficients = quadraticValues
                            .Select(lq => lq.Select(q => new QuadraticCoeficient() { Value = q }).ToList()).ToList();

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
    }
}
