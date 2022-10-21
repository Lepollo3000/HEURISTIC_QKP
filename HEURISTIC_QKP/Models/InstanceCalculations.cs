
namespace HEURISTIC_QKP.Models
{
    public class InstanceCalculations
    {
        public InstanceRelation[] Relations { get; set; } = null!;

        public InstanceCalculations(Instance instance)
        {
            float[] listRelationWeightProfit = GetInstanceRWP(instance.LinearCoeficients);
            int[] listRelationProductProduct = GetInstanceRPP(instance.QuadraticCoeficients);
            float[] listRelationOfRelations = GetInstanceRR(instance.NumberCoeficients, listRelationWeightProfit, listRelationProductProduct);

            InstanceRelation[] instanceRelations = new InstanceRelation[instance.NumberCoeficients];

            for (int i = 0; i < instance.LinearCoeficients.Count(); i++)
            {
                //float relationOfRelations = listRelationProductProduct[i] / listRelationWeightProfit[i];

                // MERGE THE LINEAR COEFICIENT, RWP, RPP AND RR IN AN OBJECT OF INSTANCE RELATION
                InstanceRelation relation = new InstanceRelation()
                {
                    LinearCoeficient = instance.LinearCoeficients[i],
                    RelationWeightProfit = listRelationWeightProfit[i],
                    RelationProductProduct = listRelationProductProduct[i],
                    RelationOfRelations = listRelationOfRelations[i]
                };

                // ADD THE INSTANCE RELATION TO THE ARRAY
                instanceRelations[i] = relation;
            }

            // SORT THE INSTANCE RELATIONS ARRAY
            instanceRelations = instanceRelations.OrderBy(c => c.RelationOfRelations).ToArray();

            // SAVE THE INSTANCE RELATIONS ARRAY IN THE MAIN ARRAY
            Relations = instanceRelations;
        }

        private int[] GetInstanceRPP(QuadraticCoeficient[,] quadraticCoeficients)
        {
            int[] relationProductProduct = new int[quadraticCoeficients.GetLength(0)];

            // SUM ALL EXTRA PROFITS OF COMBINATIONS OF EACH LINEAR COEFICIENT WITH ALL THE OTHERS
            for (int i = 0; i < quadraticCoeficients.GetLength(0); i++)
            {
                // INITIALIZE THE SUM
                int sum = 0;

                for (int j = 0; j < quadraticCoeficients.GetLength(1); j++)
                {
                    // SUM THE EXTRA PROFIT
                    sum += quadraticCoeficients[i, j].ExtraProfit;
                }

                // ADD THE SUM TO THE ARRAY
                relationProductProduct[i] = sum;
            }

            // RETURN THE ARRAY
            return relationProductProduct;
        }

        private float[] GetInstanceRWP(LinearCoeficient[] linearCoeficients)
        {
            // DIVISION OF PROFIT[I] BY WEIGHT[I]
            return linearCoeficients.Select(cl => (float)cl.Profit / (float)cl.Weight).ToArray();
        }

        private float[] GetInstanceRR(int numberCoeficients, float[] rwp, int[] rpp)
        {
            float[] relationsOfRelations = new float[numberCoeficients];

            // DIVISION OF RPP[I] BY RWP[I]
            for (int i = 0; i < numberCoeficients; i++)
            {
                // ADD THE DIVISION TO THE ARRAY
                relationsOfRelations[i] = rpp[i] / rwp[i];
            }

            // RETURN THE ARRAY
            return relationsOfRelations;
        }
    }
}
